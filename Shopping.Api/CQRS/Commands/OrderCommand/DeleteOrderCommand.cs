using MediatR;
using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Api.CQRS.Commands.OrderCommand
{

    public class DeleteOrderCommand : IRequest<RequestResponse>
    {   
        public int Id { get; set; }
    }
}
