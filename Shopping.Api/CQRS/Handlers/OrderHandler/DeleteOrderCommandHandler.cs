using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Commands.OrderCommand;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.OrderHandler
{

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, RequestResponse>
    {
        private readonly ProductContext dbContext;
   

        public DeleteOrderCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }
   
        public async Task<RequestResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {

            RequestResponse response;

            var entity = await dbContext.Orders
                .Include(o => o.OrderItems)
                .TagWith(nameof(DeleteOrderCommandHandler))
                .FirstOrDefaultAsync(d => d.Id == request.Id);
            if (entity == null) throw new Exception("The order does not exists");

            dbContext.Orders.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            response = RequestResponse.Success(entity.Id);

            return response;
     
        }
    }
}
