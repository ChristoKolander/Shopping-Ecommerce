using Microsoft.AspNetCore.Mvc;
using Shopping.Api.Extensions;
using Shopping.Api.LoggerService;
using Shopping.Api.Repositories.Interfaces;
using Shopping.Models.Dtos.CRUDs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        #region Fields and CTOR

        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;
        private readonly ILoggerManager logger;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
                                      IProductRepository productRepository,
                                      ILoggerManager logger)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            this.logger = logger;
        }

        #endregion

        [HttpGet]
        [Route("{userId}/GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(int userId)
        {

            var cartItems = await shoppingCartRepository.GetCartItems(userId);

            if (cartItems == null)
            {
                logger.LogError("CartItems could not be found in database");
                return NoContent();
            }

            var products = await productRepository.GetProducts();

            if (products == null)
            {
                logger.LogError("No products exist in the system");
                throw new Exception("No products exist in the system");
            }

            var cartItemsDto = cartItems.ConvertToDto(products);

            return Ok(cartItemsDto);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
        {
          
            var cartItem = await shoppingCartRepository.GetCartItem(id);

                if (cartItem == null)
                {
                    logger.LogError("CartItem could not be found.");
                    return NotFound();
                }

            var product = await productRepository.GetProduct(cartItem.ProductId);

                if (product == null)
                {
                    logger.LogError("Product could not be found.");
                    return NotFound();
                }

            var cartItemDto = cartItem.ConvertToDto(product);

                    return Ok(cartItemDto); 
           
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> AddCartItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            
            var newCartItem = await shoppingCartRepository.AddCartItem(cartItemToAddDto);

                if (newCartItem == null)
                {
                    logger.LogError("CartItem could not be added.");
                    return NoContent();
                }

            var product = await productRepository.GetProduct(newCartItem.ProductId);

                if (product == null)
                {
                    logger.LogError($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
                    throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
                }

            var newCartItemDto = newCartItem.ConvertToDto(product);

                    return CreatedAtAction(nameof(GetCartItem), new { id = newCartItemDto.Id }, newCartItemDto);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteCartItem(int id)
        {
           
            var cartItem = await shoppingCartRepository.DeleteCartItem(id);

                if (cartItem == null)
                {
                    logger.LogError("CartItem could not be found.");
                    return NotFound();
                }

            var product = await productRepository.GetProduct(cartItem.ProductId);

                if (product == null)
                {
                    logger.LogError("Product could not be found.");
                    return NotFound();

                }
               

            var cartItemDto = cartItem.ConvertToDto(product);

                    return Ok(cartItemDto);

        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {

            var cartItem = await shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);

            if (cartItem == null)
            {
                logger.LogError("CartItem could not be found.");
                return NotFound();
            }


            var product = await productRepository.GetProduct(cartItem.ProductId);

            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);

        }

    }
}
