using MediatR;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Api.CQRS.Commands.ProductCommand
{
    public class CreateProductCommand : IRequest<RequestResponse>
    {
        public CreateProductCommand()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; }
        public int Qty { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }

    }
}