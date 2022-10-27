using MediatR;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Queries.OrderQuery
{

    public class GetOrderById : IRequest<QueryResult<QueryResponse>>
    {   
        public int Id { get; set; }
        public string UserEmail { get; set; }
    }
}
