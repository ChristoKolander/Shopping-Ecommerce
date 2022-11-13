using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.QueryHandler
{ 
    public class GetMyOrderDetailsHandler : IRequestHandler<GetMyOrderDetails, OrderDto>
    {
        private readonly ProductContext dbContext;

        public GetMyOrderDetailsHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
  
        }

        public async Task<OrderDto> Handle(GetMyOrderDetails request, CancellationToken cancellationToken)
        {
            var order = await dbContext.Orders
              .TagWith(nameof(GetMyOrderDetails))
              .Where(x => x.UserName == request.UserName && x.Id == request.OrderId)
              .FirstOrDefaultAsync();

            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                OrderDate = order.OrderDate,
                OrderProducts = order.OrderItems.Select(o => new OrderItemDto
                {
                    ImageUrl = o.ProductOrdered.ImageUrl,
                    ProductId = o.ProductOrdered.ProductId,
                    ProductName = o.ProductOrdered.ProductName,
                    UnitPrice = o.UnitPrice,
                    Units = o.Units
                }).ToList(),
                OrderNumber = order.Id,
                ShippingAddress = order.ShipToAddress,
                Total = order.Total()
            };

        }

    }
}
