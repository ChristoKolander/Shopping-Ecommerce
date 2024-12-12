using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.Api.CQRS.Commands.ProductCommand;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.ProductHandler
{
    public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, RequestResponse>
    {
        private readonly ProductContext dbContext;

        public UpdateProductCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public async Task<RequestResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            RequestResponse response;

            var entity = await dbContext.Products
                .TagWith(nameof(UpdateProductCommandHandler))
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (entity == null) throw new Exception("The Product does NOT exists");

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.Qty = request.Qty;
            entity.ProductCategoryId = request.ProductCategoryId;

            dbContext.Products.Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            response = RequestResponse.Success(entity.Id);

            return response;

        }

    }
}
