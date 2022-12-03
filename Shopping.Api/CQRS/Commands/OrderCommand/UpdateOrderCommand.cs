using MediatR;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Commands.OrderCommand
{

    public class UpdateOrderCommand : IRequest<RequestResponse>
    {      
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public int TotalCost { get; set; }
    }
}
