using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain
{
    public class CustomersWithOrders : CustomerAddress
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int SalesOrderID { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ? ShipDate { get; set; }
        public string AccountNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public int Count { get; set; }

        public int SalesOrderDetailID { get; set; }
        public int OrderQty { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string CarrierTrackingNumber { get; set; }

    }

    public class CustomerAddress
    {
        public int PersonId { get; set; }
        public int AddressTypeID { get; set; }
        public int AddressID { get; set; }
        public string FullAddress { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int StateProvinceID { get; set; }
        

    }
}
