using MediatR;
using Shopping.Core;

namespace Shopping.Api.CQRS.Queries.OrderQuery
{
    public class GetMyOrders : IRequest<IEnumerable<OrderDto>>
    {

        public string UserEmail { get; set; }
        public string UserName { get; set; }

        public GetMyOrders(string userName)
        {
            UserName = userName;
     
        }
    }

}
