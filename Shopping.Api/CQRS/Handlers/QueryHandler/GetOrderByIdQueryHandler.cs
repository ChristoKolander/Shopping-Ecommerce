using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;


namespace Shopping.Api.CQRS.Handlers.QueryHandler.OrderHandler
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderById, QueryResult<QueryResponse>>
    {
        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetOrderByIdQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QueryResult<QueryResponse>> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            QueryResult<QueryResponse> response;

            var result = dbContext.Orders
                .TagWith(nameof(GetOrderByIdQueryHandler))
                .Where(x => x.UserEmail == request.UserEmail && x.Id == request.Id)
                .ProjectTo<QueryResponse>(mapper.ConfigurationProvider)
                .FirstOrDefault();
  
            response = new QueryResult<QueryResponse>
            {
                Successful = true,
                Item = result ?? new QueryResponse()
            };

            return await Task.FromResult(response);
        }



    }   

    }






