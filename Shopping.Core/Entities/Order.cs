using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Core.Entities
{
    public class Order : BaseEntity<int>
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public string OrderName { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalCost { get; set; }

        public string CustomerId { get; set; }
        public Address ShipToAddress { get; set; }

        public OrderItem OrderItem { get; set; }

        public int OrderItemId {get;set;}

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order() { }

        public Order(string customerId, Address shipToAddress, List<OrderItem> orderItems)
        {

            CustomerId = customerId;
            ShipToAddress = shipToAddress;
            OrderItems = orderItems;
        }

        public decimal Total()
        {
            var total = 0m;
            foreach (var item in OrderItems)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }

    }

   
}