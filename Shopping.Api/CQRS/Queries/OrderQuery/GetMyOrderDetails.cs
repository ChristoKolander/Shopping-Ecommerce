
using MediatR;

namespace Shopping.Api.CQRS.Queries.OrderQuery
{
    public class GetMyOrderDetails : IRequest<OrderDto>
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
     

        public GetMyOrderDetails(string userName, int orderId)
        {
            UserEmail = userName;
            OrderId = orderId;
        }
    }

}



