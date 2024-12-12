using MediatR;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities;

namespace Shopping.Api.CQRS.Queries.ProductQuery
{
    public class GetProductById : IRequest<QueryResult<Product>>
    {
        public int Id { get; set; }
        
        public GetProductById()
        {
        } 

    }
}
