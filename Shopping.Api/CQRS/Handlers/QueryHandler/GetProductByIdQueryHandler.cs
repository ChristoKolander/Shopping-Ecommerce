using AutoMapper;
using MediatR;
using Shopping.Api.CQRS.Queries.ProductQuery;
using Shopping.Core.Entities;
using Shopping.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.Query;


namespace Shopping.Api.CQRS.Handlers.QueryHandler
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductById, QueryResult<Product>>
    {
        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QueryResult<Product>> Handle(GetProductById request, CancellationToken cancellationToken)
       
        { 
            QueryResult<Product> response;

            var result = await dbContext.Products
                .Include(p => p.ProductCategory)
                .TagWith(nameof(GetProductByIdQueryHandler))
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync();

            response = new QueryResult<Product>
            {
                Successful = true,
                Item = result ?? new Product()
            };

            return await Task.FromResult(response);
        }
    }
}



