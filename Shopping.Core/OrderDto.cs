using Shopping.Core.Entities;
using System;
using System.Collections.Generic;

namespace Shopping.Core
{ 
    public class OrderDto
    {
        private const string DEFAULT_STATUS = "Pending";

        public int OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public int TotalCost { get; set; }
        public decimal Total { get; set; }
        public string Status => DEFAULT_STATUS;
        public Address ShippingAddress { get; set; }
        public List<OrderItemDto> OrderProducts { get; set; } = new List<OrderItemDto>();

    }
}
