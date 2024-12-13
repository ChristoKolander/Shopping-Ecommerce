using MediatR;
using Shopping.Api.CQRS.Queries.Query;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Queries.ProductQuery
{
    public class GetProducts: IRequest<QueryResult<Product>>
    {
        public GetProducts() { }
    }
}
