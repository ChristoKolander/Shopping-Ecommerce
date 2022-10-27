using MediatR;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.Queries.OrderQuery
{

    public class GetOrders : IRequest<QueryResult<QueryResponse>>
    {
        public string UserEmail { get; set; }
    }
}
