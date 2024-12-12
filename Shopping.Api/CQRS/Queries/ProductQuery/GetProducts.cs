using MediatR;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Queries.ProductQuery
{
    public class GetProducts: IRequest<QueryResult<Product>>
    {
        public GetProducts() { }
    }
}
