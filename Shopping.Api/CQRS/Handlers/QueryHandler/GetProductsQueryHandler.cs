using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Api.CQRS.Queries.ProductQuery;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.QueryHandler
{
    public class GetProductsQueryHandler : IRequestHandler<GetProducts, QueryResult<Product>>
    {

        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetProductsQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public async Task<QueryResult<Product>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            QueryResult<Product> response;

            var result = await dbContext.Products
                .Include(p => p.ProductCategory)
                .TagWith(nameof(GetProductsQueryHandler))
                .ToListAsync();

            response = new QueryResult<Product>
            {
                Successful = true,
                Items = result ?? new List<Product>()
            };

            return await Task.FromResult(response);

        }
    }
}


  