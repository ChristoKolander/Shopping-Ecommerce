using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;


namespace Shopping.Api.CQRS.Handlers.QueryHandler.OrderHandler
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderById, QueryResult<OrderResponse>>
    {
        private readonly ProductContext dbContext;
        private readonly IMapper mapper;

        public GetOrderByIdQueryHandler(ProductContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<QueryResult<OrderResponse>> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            QueryResult<OrderResponse> response;

            var result = dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.ProductOrdered)
                .TagWith(nameof(GetOrderByIdQueryHandler))
                .Where(x => x.UserEmail == request.UserEmail && x.Id == request.Id)
 
       
                .ProjectTo<OrderResponse>(mapper.ConfigurationProvider)
             
                .FirstOrDefault();
  
            response = new QueryResult<OrderResponse>
            {
                Successful = true,
                Item = result ?? new OrderResponse()
            };

            return await Task.FromResult(response);
        }



    }   

    }






