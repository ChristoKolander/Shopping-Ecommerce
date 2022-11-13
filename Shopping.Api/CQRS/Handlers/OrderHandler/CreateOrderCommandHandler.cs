using MediatR;
using Shopping.Api.CQRS.Commands.OrderCommand;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;



namespace Shopping.Api.CQRS.Handlers.OrderHandler
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, RequestResponse>
    {

        private readonly ProductContext dbContext;

        public CreateOrderCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<RequestResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            RequestResponse response;
         
            var entity = new Order
            {
                CustomerId = request.UserId,
                UserEmail = request.UserEmail,
                UserName = request.UserName,
                OrderNumber = request.OrderNumber,
                OrderName = "Order Name Something? " + Guid.NewGuid(),
                OrderDate = request.OrderDate,
                OrderItems = request.OrderItems,
                TotalCost = request.TotalCost,
                ShipToAddress = request.ShippingAddress

                            
            };

            dbContext.Orders.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            response = RequestResponse.Success(entity.Id);
            
            return response;
        }

    }
}
