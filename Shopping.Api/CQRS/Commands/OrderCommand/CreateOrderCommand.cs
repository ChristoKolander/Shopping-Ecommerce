using MediatR;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using System;

namespace Shopping.Api.CQRS.Commands.OrderCommand
{

    public class CreateOrderCommand : IRequest<RequestResponse>
    {
        public string UserId { get; set; }
        public string CustomerId { get; set; } 

        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }     
        public int TotalCost { get; set; }      
        public List<OrderItem> OrderItems { get; set; }
        public Address ShippingAddress { get; set; }
       
                
        public List<ProductOrdered> ProductsOrdered { get; set; } = new List<ProductOrdered>();

      
    }
}
