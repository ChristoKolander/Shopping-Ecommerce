using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Api.Queries.OrderQuery;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.QueryHandler.OrderHandler
{

    public class GetOrdersQueryHandler : IRequestHandler<GetOrders, QueryResult<QueryResponse>>
    {
        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetOrdersQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QueryResult<QueryResponse>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            QueryResult<QueryResponse> response;

     
            var result = dbContext.Orders
                .TagWith(nameof(GetOrdersQueryHandler))
                .Where(x => x.UserEmail == request.UserEmail)
                .ProjectTo<QueryResponse>(mapper.ConfigurationProvider)
                .ToList();

                response = new QueryResult<QueryResponse>
                {
                    Successful = true,
                    Items = result ?? new List<QueryResponse>()
                };
                
                return await Task.FromResult(response);
        }
    }
}

