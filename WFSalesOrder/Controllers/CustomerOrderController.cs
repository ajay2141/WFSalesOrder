using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using BusinessDomain;
using PagedList;
using System.Web.Routing;
using WFSalesOrder.Models;


namespace WFSalesOrder.Controllers
{
    public class CustomerOrderController : Controller
    {
        // GET: CustomerOrder
        public ActionResult Index()
        {
            return RedirectToAction("DisplayPremierCustomers", new { sortOrder = "", CurrentSort =""});
        }

        public ActionResult DisplayPremierCustomers(string sortOrder, string CurrentSort, int? page)
        {
            Sales _Sales = new Sales();
            int pageSize = 20;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<CustomersWithOrders> Customers = null;
            Customers = _Sales.getPremierCustomers(pageIndex, pageSize);
            return View(Customers);
        }

        public ActionResult PremierCustomersOrders(int CustomerID, int? page)
        {
            Sales _Sales = new Sales();
            int pageSize = 20;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<CustomersWithOrders> Customers = null;
            Customers = _Sales.getPremierCustomersOrders(pageIndex, pageSize, CustomerID);
            return View(Customers);
          
        }
        public ActionResult PremierCustomersOrdersDetail(int CustomerID, int SalesOrderID, int? page)
        {
            Sales _Sales = new Sales();
            int pageSize = 20;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<CustomersWithOrders> Customers = null;
            Customers = _Sales.getPremierCustomersOrdersDetail(pageIndex, pageSize, CustomerID, SalesOrderID);
            return View(Customers);

        }

        public ActionResult ChangeShipingAddress(int CustomerID, int SalesOrderID)
        {
            ViewBag.CustomerID = CustomerID;
            ViewBag.SalesOrderID = SalesOrderID;
            Sales _Sales = new Sales();
            List<CustomerAddress> CustomersAddress = null;
            CustomersAddress = _Sales.getCustomersAddress(CustomerID);
            return View(CustomersAddress);
        }
        public ActionResult UpdateBillToAddress(int CustomerID, int SalesOrderID,int AddressID)
        {
            Sales _Sales = new Sales();
            var _result = _Sales.ChangeShipingAddress(CustomerID,SalesOrderID, AddressID);
            return RedirectToAction("PremierCustomersOrdersDetail", new { CustomerID = CustomerID, SalesOrderID= SalesOrderID });
        }

        public ActionResult AddIteamToOrder(int CustomerID,int SalesOrderID)
        {
            Sales _Sales = new Sales();
            List<Product> Products= _Sales.getProducts();
            ViewBag.CustomerID = CustomerID;
            ViewBag.SalesOrderID = SalesOrderID;
            ViewBag.ProductList = Products;
            return View();
        }
        
        public ActionResult SaveNewItemToOrder(SalesOrderDetail pSalesOrderDetail, int CustomerID, int SalesOrderID)
        {
            Sales _Sales = new Sales();

            int ProductID =Int32.Parse(Request.Form["ProductList"].ToString());
            Product _Product = _Sales.getProducts(ProductID);
            int _SpecialOfferID = _Sales.getSpecialOfferID(ProductID);

            SalesOrderDetail _SalesOrderDetail = new SalesOrderDetail();
            _SalesOrderDetail.SalesOrderID = SalesOrderID;
            _SalesOrderDetail.CarrierTrackingNumber = pSalesOrderDetail.CarrierTrackingNumber;
            _SalesOrderDetail.OrderQty = pSalesOrderDetail.OrderQty;
            _SalesOrderDetail.ProductID = ProductID;
            _SalesOrderDetail.SpecialOfferID = _SpecialOfferID;
            _SalesOrderDetail.UnitPrice = _Product.ListPrice;
            _SalesOrderDetail.UnitPriceDiscount = Decimal.Parse("0.10");
            _SalesOrderDetail.LineTotal = (pSalesOrderDetail.OrderQty * _Product.ListPrice)- (pSalesOrderDetail.OrderQty * _Product.ListPrice * _SalesOrderDetail.UnitPriceDiscount);
            _SalesOrderDetail.ModifiedDate = DateTime.Now;
            var _result = _Sales.SaveNewItemToOrder(CustomerID, _SalesOrderDetail);
            return RedirectToAction("PremierCustomersOrdersDetail", new { CustomerID = CustomerID, SalesOrderID = SalesOrderID });
        }

        public ActionResult DispayCustomers(string sortOrder, string CurrentSort, int? page)
        {
            Sales _Sales = new Sales();
            int pageSize = 20;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "CustomerID" : sortOrder;
            IPagedList<CustomersWithOrders> Customers = null;
            Customers=_Sales.getCustomersDetail(pageIndex, pageSize);
            return View(Customers);
        }
        public ActionResult OrderDetails(string CustomerID, string SalesOrderID)
        {
            return View();
        }

        public ActionResult ModifyItemFromOrder(string CustomerID, string SalesOrderID, int SalesOrderDetailID)
        {
            Sales _Sales = new Sales();
            SalesOrderDetail _SalesOrderDetail;
            _SalesOrderDetail=_Sales.GetItemDetil(SalesOrderDetailID);
            ViewBag.CustomerID = CustomerID;
            ViewBag.SalesOrderID = SalesOrderID;
            return View(_SalesOrderDetail);
        }

        public ActionResult DeleteItemFromOrder(string CustomerID, string SalesOrderID,int SalesOrderDetailID)
        {
            Sales _Sales = new Sales();
            SalesOrderDetail _SalesOrderDetail;
            ViewBag.CustomerID = CustomerID;
            ViewBag.SalesOrderID = SalesOrderID;
            _SalesOrderDetail = _Sales.GetItemDetil(SalesOrderDetailID);
            return View(_SalesOrderDetail);
        }
        [HttpPost]
        public ActionResult DeleteItemInDB(SalesOrderDetail pSalesOrderDetail, int pCustomerID,int pSalesOrderID)
        {
            Sales _Sales = new Sales();
            bool _result = _Sales.DeleteItemInDB(pSalesOrderDetail.SalesOrderDetailID);
            return RedirectToAction("PremierCustomersOrdersDetail", new { CustomerID = pCustomerID, SalesOrderID = pSalesOrderID });
        }

        [HttpPost]
        public ActionResult UpdateItemInDB(SalesOrderDetail pSalesOrderDetail, int pCustomerID, int pSalesOrderID)
        {
            Sales _Sales = new Sales();
            var _result = _Sales.UpdateItemInDB(pSalesOrderDetail);
            return RedirectToAction("PremierCustomersOrdersDetail", new {CustomerID= pCustomerID, SalesOrderID= pSalesOrderID });
        }

    }
}