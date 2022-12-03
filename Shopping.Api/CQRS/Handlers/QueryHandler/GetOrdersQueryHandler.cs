using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.QueryHandler.OrderHandler
{

    public class GetOrdersQueryHandler : IRequestHandler<GetOrders, QueryResult<OrderResponse>>
    {
        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetOrdersQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QueryResult<OrderResponse>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            QueryResult<OrderResponse> response;

     
            var result = await dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.ProductOrdered)
                .TagWith(nameof(GetOrdersQueryHandler))
                .Where(x => x.UserEmail == request.UserEmail)
                .ProjectTo<OrderResponse>(mapper.ConfigurationProvider)
                .ToListAsync();

                response = new QueryResult<OrderResponse>
                {
                    Successful = true,
                    Items = result ?? new List<OrderResponse>()
                };
                
                return await Task.FromResult(response);
        }
    }
}

