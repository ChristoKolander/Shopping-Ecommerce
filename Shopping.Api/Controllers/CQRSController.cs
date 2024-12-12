using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Api.CQRS.Queries.ProductQuery;
using Shopping.Api.CQRS.Commands.ProductCommand;

namespace Shopping.Api.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase
    {

        private IMediator mediator;

        public CQRSController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneProductUsingCQRS(int id)
        {
            var result = await mediator.Send(new GetProductById { Id = id });
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetProductsUsingCQRS()
        {
            var result = await mediator.Send(new GetProducts{});
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpPatch]
        public async Task<IActionResult> UpdateProductUsingCQRS([FromBody] UpdateProductCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProductUsingCQRS([FromBody] CreateProductCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteProductUsingCQRS(DeleteProductCommand command)
        {
            var result = await mediator.Send(command);
            return result.Successful == true
                ? Ok(result)
                : BadRequest(result);
        }

    }
}

      
