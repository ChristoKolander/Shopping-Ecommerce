using MediatR;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using System;

namespace Shopping.Api.CQRS.Commands.OrderCommand
{

    public class CreateOrderCommand : IRequest<RequestResponse>
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalCost { get; set; }

        public Address ShippingAddress { get; set; }
        //public List<Product> Products { get; set; } = new List<Product>();
        //public List<ProductOrdered> ProductOrdered { get; set; } = new List<ProductOrdered>();

        public List<OrderProduct> OrderProducts = new List<OrderProduct>();
    }
}
//{
//    "userId": "02592be0-dde1-482c-a904-c56a750a1cbf",
//  "userEmail": "TestUser1@hotmail.com",
//  "orderItems": "Phone",
//  "orderDate": "2022-10-18T11:49:11.724Z",
//  "totalCost": 10,
//  "shippingAddress": {
//        "id": 1,
//    "street": "Street",
//    "city": "Yes",
//    "state": "UA",
//    "country": "USA",
//    "zipCode": "5555"
//  }
//}