using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Commands.OrderCommand;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

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

                var entity = dbContext.Orders
                    .TagWith(nameof(DeleteOrderCommandHandler))
                    .SingleOrDefault(d => d.Id == request.Id);
                if (entity == null) throw new Exception("The order does not exists");

                dbContext.Orders.Remove(entity);
                await dbContext.SaveChangesAsync(cancellationToken);

                response = RequestResponse.Success(entity.Id);
         
            return response;
        }
    }
}
