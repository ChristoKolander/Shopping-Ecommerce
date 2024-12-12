using MediatR;
using Shopping.Api.CQRS.Commands.ProductCommand;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Infrastructure.Data;

namespace Shopping.Api.CQRS.Handlers.ProductHandler
{

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, RequestResponse>
    {
        private readonly ProductContext dbContext;

        public CreateProductCommandHandler(ProductContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task<RequestResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            RequestResponse response;

            var entity = new Product
            {
                Name = request.Name,
                Description = request.Description,
                ImageURL = request.ImageURL,
                Price = request.Price,
                ProductCategoryId = request.ProductCategoryId,
                Qty = request.Qty,
                
            };

            dbContext.Products.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            response = RequestResponse.Success(entity.Id);

            return response;
        }
    } 
}