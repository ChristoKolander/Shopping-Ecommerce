using MediatR;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Commands.ProductCommand
{
    public class UpdateProductCommand : IRequest<RequestResponse>
    {

        public UpdateProductCommand()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; }
        public int Qty { get; set; }

    } 
}