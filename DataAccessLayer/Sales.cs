using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDomain;
using PagedList;

namespace DataAccessLayer
{
    public class Sales
    {
        public IPagedList<CustomersWithOrders> getCustomersDetail(int pagenumber, int pageSize)
        {
            IPagedList<CustomersWithOrders> _customers = null;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var query = from c in db.Customers
                            join cd in db.CustomerPIIs on c.CustomerID equals cd.CustomerID
                            join sh in db.SalesOrderHeaders on c.CustomerID equals sh.CustomerID
                            join sd in db.SalesOrderDetails on sh.SalesOrderID equals sd.SalesOrderID
                            group new { c, cd, sh } by new
                            {
                                c.CustomerID,
                                cd.FirstName,
                                cd.LastName,
                                cd.EmailAddress,
                                cd.PhoneNumber,
                                sh.SalesOrderID,
                                sh.OrderDate,
                                sh.ShipDate,
                                sh.AccountNumber,
                                sh.SubTotal,
                                sh.TaxAmt,
                                sh.Freight,
                                sh.TotalDue
                            } into grp
                            where grp.Count() > 19
                            select new CustomersWithOrders()
                            {
                                Count = grp.Count(),
                                CustomerID = grp.Key.CustomerID,
                                FirstName = grp.Key.FirstName,
                                LastName = grp.Key.LastName,
                                EmailAddress = grp.Key.EmailAddress,
                                PhoneNumber = grp.Key.PhoneNumber,
                                SalesOrderID = grp.Key.SalesOrderID,
                                OrderDate = grp.Key.OrderDate,
                                ShipDate = grp.Key.ShipDate,
                                AccountNumber = grp.Key.AccountNumber,
                                SubTotal = grp.Key.SubTotal,
                                TaxAmt = grp.Key.TaxAmt,
                                Freight = grp.Key.Freight,
                                TotalDue = grp.Key.TotalDue,
                            };
                query = query.OrderBy(CustomersWithOrders => CustomersWithOrders.CustomerID);
                _customers = query.ToPagedList(pagenumber, pageSize);
                //.Skip(pagenumber * pageSize).Take(pageSize);
                // query.ToList<CustomersWithOrders>();

            }
            return _customers;
        }

        /// <summary>
        /// List of customer having more than 19 sales order
        /// </summary>
        /// <returns></returns>
        public IPagedList<CustomersWithOrders> getPremierCustomers(int pagenumber, int pageSize)
        {
            IPagedList<CustomersWithOrders> _customers = null;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var query = from c in db.Customers
                            join cd in db.CustomerPIIs on c.CustomerID equals cd.CustomerID
                            join sh in db.SalesOrderHeaders on c.CustomerID equals sh.CustomerID
                            //join be in db.BusinessEntityAddresses on c.CustomerID equals be.BusinessEntityID  
                            join a in db.Addresses on sh.BillToAddressID equals a.AddressID
                            group new { c, cd, sh, a } by new
                            {
                                c.CustomerID,
                                cd.FirstName,
                                cd.LastName,
                                cd.EmailAddress,
                                cd.PhoneNumber,
                                a.AddressLine1,
                                a.AddressLine2,
                                a.City,
                                a.PostalCode,
                                a.AddressID
                            } into grp
                            where grp.Count() > 19
                            select new CustomersWithOrders()
                            {
                                Count = grp.Count(),
                                CustomerID = grp.Key.CustomerID,
                                FirstName = grp.Key.FirstName,
                                LastName = grp.Key.LastName,
                                EmailAddress = grp.Key.EmailAddress,
                                PhoneNumber = grp.Key.PhoneNumber,
                                FullAddress = grp.Key.AddressLine1 + " " + grp.Key.AddressLine2 + " " + grp.Key.City + " " + grp.Key.PostalCode,
                                AddressID = grp.Key.AddressID
                            };
                query = query.OrderBy(CustomersWithOrders => CustomersWithOrders.CustomerID);
                _customers = query.ToPagedList(pagenumber, pageSize);
            }
            return _customers;
        }


        /// <summary>
        /// Get customers Orders
        /// </summary>
        /// <param name="pagenumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="pCustomerID"></param>
        /// <returns></returns>
        public IPagedList<CustomersWithOrders> getPremierCustomersOrders(int pagenumber, int pageSize, int pCustomerID)
        {
            IPagedList<CustomersWithOrders> _customers = null;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var query = from c in db.Customers
                            join cd in db.CustomerPIIs on c.CustomerID equals cd.CustomerID
                            join sh in db.SalesOrderHeaders on c.CustomerID equals sh.CustomerID
                            //join be in db.BusinessEntityAddresses on c.CustomerID equals be.BusinessEntityID
                            join a in db.Addresses on sh.BillToAddressID equals a.AddressID
                            where c.CustomerID == pCustomerID
                            select new CustomersWithOrders()
                            {
                                CustomerID = c.CustomerID,
                                FirstName = cd.FirstName,
                                LastName = cd.LastName,
                                EmailAddress = cd.EmailAddress,
                                PhoneNumber = cd.PhoneNumber,
                                SalesOrderID = sh.SalesOrderID,
                                OrderDate = sh.OrderDate,
                                ShipDate = sh.ShipDate,
                                AccountNumber = sh.AccountNumber,
                                SubTotal = sh.SubTotal,
                                TaxAmt = sh.TaxAmt,
                                Freight = sh.Freight,
                                TotalDue = sh.TotalDue,
                                FullAddress = a.AddressLine1 + " " + a.AddressLine2 + " " + a.City + " " + a.PostalCode,
                                AddressID = a.AddressID                                
                            };
                query = query.OrderBy(CustomersWithOrders => CustomersWithOrders.CustomerID);
                _customers = query.ToPagedList(pagenumber, pageSize);
            }
            return _customers;
        }

        /// <summary>
        /// Get Customer Order detail
        /// </summary>
        /// <param name="pagenumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="pCustomerID"></param>
        /// <param name="pSalesOrderID"></param>
        /// <returns></returns>
        public IPagedList<CustomersWithOrders> getPremierCustomersOrdersDetail(int pagenumber, int pageSize, int pCustomerID, int pSalesOrderID)
        {
            IPagedList<CustomersWithOrders> _customers = null;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var query = from c in db.Customers
                            join cd in db.CustomerPIIs on c.CustomerID equals cd.CustomerID
                            join sh in db.SalesOrderHeaders on c.CustomerID equals sh.CustomerID
                            join sd in db.SalesOrderDetails on sh.SalesOrderID equals sd.SalesOrderID
                            //join be in db.BusinessEntityAddresses on c.CustomerID equals be.BusinessEntityID
                            join a in db.Addresses on sh.BillToAddressID equals a.AddressID
                            join p in db.Products on sd.ProductID equals p.ProductID
                            where c.CustomerID == pCustomerID && sh.SalesOrderID == pSalesOrderID
                            select new CustomersWithOrders()
                            {
                                CustomerID = c.CustomerID,
                                FirstName = cd.FirstName,
                                LastName = cd.LastName,
                                EmailAddress = cd.EmailAddress,
                                PhoneNumber = cd.PhoneNumber,
                                SalesOrderID = sh.SalesOrderID,
                                OrderDate = sh.OrderDate,
                                ShipDate = sh.ShipDate,
                                AccountNumber = sh.AccountNumber,
                                SubTotal = sh.SubTotal,
                                TaxAmt = sh.TaxAmt,
                                Freight = sh.Freight,
                                TotalDue = sh.TotalDue,
                                SalesOrderDetailID = sd.SalesOrderDetailID,
                                OrderQty = sd.OrderQty,
                                ProductID = sd.ProductID,
                                ProductName = p.Name,
                                UnitPrice = sd.UnitPrice,
                                FullAddress = a.AddressLine1 + " " + a.AddressLine2 + " " + a.City + " " + a.PostalCode,
                                AddressID = a.AddressID,
                                CarrierTrackingNumber =sd.CarrierTrackingNumber
                            };
                query = query.OrderBy(CustomersWithOrders => CustomersWithOrders.CustomerID);
                _customers = query.ToPagedList(pagenumber, pageSize);
            }
            return _customers;
        }
        public List<CustomerAddress> getCustomersAddress(int pCustomerID)
        {
            List<CustomerAddress> _customersAddress = null;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var query = from c in db.Customers
                            join be in db.BusinessEntityAddresses on c.CustomerID equals be.BusinessEntityID
                            join a in db.Addresses on be.AddressID equals a.AddressID
                            where c.CustomerID == pCustomerID
                            select new CustomerAddress()
                            {
                                AddressLine1 = a.AddressLine1,
                                AddressLine2 = a.AddressLine2,
                                City = a.City,
                                PostalCode = a.PostalCode,
                                AddressID = a.AddressID,
                                StateProvinceID = a.StateProvinceID
                            };
                query = query.OrderBy(CustomerAddress => CustomerAddress.AddressID);
                _customersAddress = query.ToList<CustomerAddress>();
            }
            return _customersAddress;
        }
        public bool ChangeShipingAddress(int pCustomerID, int pSalesOrderID, int pAddressID)
        {
            var _result = false;
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                SalesOrderHeader _SalesOrderHeaders = db.SalesOrderHeaders.FirstOrDefault(s => s.SalesOrderID == pSalesOrderID);

                _SalesOrderHeaders.BillToAddressID = pAddressID;

                db.SaveChanges();
                _result = true;
            }
            return _result;
        }

        public List<Product> getProducts()
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                List<Product> _Products;
                var query = from p in db.Products
                            join sp in db.SpecialOfferProducts on p.ProductID equals sp.ProductID
                            orderby p.ProductID
                            select p
                            ;
                          
                query = query.Take(50);
                _Products = query.ToList<Product>();

                return _Products;
            }
        }
        public Product getProducts(int pProductId)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var _Products = db.Products.Where(Product => Product.ProductID == pProductId).OrderBy(Product => Product.ProductID).Take(1);
                return _Products.FirstOrDefault();
            }
        }
        public int getSpecialOfferID(int pProductId)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var _Products = db.SpecialOfferProducts.Where(Product => Product.ProductID == pProductId).OrderBy(Product => Product.ModifiedDate).Take(1);
                var output =_Products.FirstOrDefault();
                if (output != null)
                    return output.SpecialOfferID;
                else
                    return 1;
            }
        }

        public bool SaveNewItemToOrder(int pCustomerID, SalesOrderDetail pSalesOrderDetail)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                pSalesOrderDetail.rowguid = Guid.NewGuid();
                db.SalesOrderDetails.Add(pSalesOrderDetail);

                SalesOrderHeader _SalesOrderHeader = db.SalesOrderHeaders.Where(t => t.SalesOrderID == pSalesOrderDetail.SalesOrderID).FirstOrDefault();
                _SalesOrderHeader.SubTotal = _SalesOrderHeader.SubTotal + pSalesOrderDetail.LineTotal;
                _SalesOrderHeader.TotalDue = _SalesOrderHeader.TotalDue + pSalesOrderDetail.LineTotal;

                db.SaveChanges();

                return true;
            }
        }

        public SalesOrderDetail GetItemDetil(int pSalesOrderDetailID)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                var _Products = db.SalesOrderDetails.Where(sd => sd.SalesOrderDetailID== pSalesOrderDetailID);
                return _Products.FirstOrDefault();
            }
        }

        public bool DeleteItemInDB(int pSalesOrderDetailID)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                SalesOrderDetail _SalesOrderDetail = db.SalesOrderDetails.Where(sd => sd.SalesOrderDetailID == pSalesOrderDetailID).SingleOrDefault();
                db.SalesOrderDetails.Remove(_SalesOrderDetail);
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateItemInDB(SalesOrderDetail pSalesOrderDetail)
        {
            using (var db = new AdventureWorks2016CTP3Entities())
            {
                SalesOrderDetail _SalesOrderDetail = db.SalesOrderDetails.Where(sd => sd.SalesOrderDetailID == pSalesOrderDetail.SalesOrderDetailID).SingleOrDefault();

                _SalesOrderDetail.CarrierTrackingNumber = pSalesOrderDetail.CarrierTrackingNumber;

                db.SaveChanges();
                return true;
            }
        }

    }
}
