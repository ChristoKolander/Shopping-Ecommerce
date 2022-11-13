using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.QueryHandler
{
    public class GetMyOrdersHandler : IRequestHandler<GetMyOrders, IEnumerable<OrderDto>>
    {
        private readonly ProductContext dbContext;

        public GetMyOrdersHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetMyOrders request, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
               .TagWith(nameof(GetMyOrdersHandler))
               .Where(x => x.UserName == request.UserName)
               .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                OrderDate = o.OrderDate,
                OrderProducts = o.OrderItems.Select(op => new OrderItemDto()
                {
                    ImageUrl = op.ProductOrdered.ImageUrl,
                    ProductId = op.ProductOrdered.ProductId,
                    ProductName = op.ProductOrdered.ProductName,
                    UnitPrice = op.UnitPrice,
                    Units = op.Units
                }).ToList(),
                OrderNumber = o.Id,
                ShippingAddress = o.ShipToAddress,
                Total = o.Total()
            
            });
        }
    }
}
