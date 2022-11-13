using System;
using System.Collections.Generic;

namespace Shopping.Core.Entities.CQRSresponses
{
    public class QueryResponse
    {

        private const string DEFAULT_STATUS = "Pending";

        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public int OrderNumber { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public int TotalCost { get; set; } //??
        public decimal Total { get; set; } //??

        public string Status => DEFAULT_STATUS;
        public Address ShipToAddress { get; set; }

    }
}

