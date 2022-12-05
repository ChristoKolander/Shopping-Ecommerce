using MediatR;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Queries.OrderQuery
{

    public class GetOrders : IRequest<QueryResult<OrderResponse>>
    {
        public string UserEmail { get; set; }
    }
}
