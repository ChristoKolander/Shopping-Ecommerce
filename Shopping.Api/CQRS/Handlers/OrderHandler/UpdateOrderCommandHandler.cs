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
   
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, RequestResponse>
    {
        private readonly ProductContext dbContext;

        public UpdateOrderCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<RequestResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            RequestResponse response;

        
                var entity = dbContext.Orders
                    .TagWith(nameof(UpdateOrderCommandHandler))
                    .SingleOrDefault(d => d.Id == request.Id);
                if (entity == null) throw new Exception("The order does NOT exists");

                entity.UserEmail = request.UserEmail;
                entity.OrderDate = request.OrderDate;
                entity.OrderItems = request.OrderItems;
                entity.TotalCost = request.TotalCost;

                dbContext.Orders.Update(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
                response = RequestResponse.Success(entity.Id);
                
                return response;
        }
    }
}
