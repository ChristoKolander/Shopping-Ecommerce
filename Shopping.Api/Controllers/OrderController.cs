using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Api.CQRS.Commands.OrderCommand;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Api.Queries.OrderQuery;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }

       
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(DeleteOrderCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpGet("order/{id}/{userEmail}")]   
        public async Task<IActionResult> GetOrder(int id, string userEmail)
        {
            var result = await mediator.Send(new GetOrderById { Id = id, UserEmail = userEmail });
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpGet("orders/{userEmail}")]
        public async Task<IActionResult> GetOrders(string userEmail)
        {
            var result = await mediator.Send(new GetOrders { UserEmail = userEmail });
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
