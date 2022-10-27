using System;
using System.Collections.Generic;


namespace Shopping.Core.Entities
{ 
    public class Order : BaseEntity<int>
    {       
        public string UserEmail { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderItems { get; set; }
        public int TotalCost { get; set; }


        public string CustomerId { get; set; }
        public Address ShipToAddress { get; set; }
        public string UserName { get; set; }


        public Order() { }

        public Order(string customerId, Address shipToAddress, List<OrderProduct> orderProducts)
        {

            CustomerId = customerId;
            ShipToAddress = shipToAddress;
            OrderProducts = orderProducts;
        }


        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public decimal Total()
        {
            var total = 0m;
            foreach (var item in OrderProducts)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }

    }

   
}