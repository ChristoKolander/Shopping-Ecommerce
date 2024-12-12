using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Commands.ProductCommand;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.ProductHandler
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, RequestResponse>
    {
        private readonly ProductContext dbContext;


        public DeleteProductCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<RequestResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            RequestResponse response;

            var entity = await dbContext.Products
                .TagWith(nameof(DeleteProductCommandHandler))
                .FirstOrDefaultAsync(d => d.Id == request.Id);                
            
                if (entity == null) throw new Exception("The Product does not exists");

            dbContext.Products.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            response = RequestResponse.Success(entity.Id);

            return response;

        }

    }
}
