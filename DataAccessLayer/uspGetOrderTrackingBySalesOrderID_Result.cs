//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer
{
    using System;
    
    public partial class uspGetOrderTrackingBySalesOrderID_Result
    {
        public int SalesOrderID { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public int OrderTrackingID { get; set; }
        public int TrackingEventID { get; set; }
        public string EventName { get; set; }
        public string EventDetails { get; set; }
        public System.DateTime EventDateTime { get; set; }
    }
}
