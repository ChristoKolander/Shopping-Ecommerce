using MediatR;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Commands.ProductCommand
{
    public class DeleteProductCommand : IRequest<RequestResponse>
    {
        public int Id { get; set; }
    }
}
