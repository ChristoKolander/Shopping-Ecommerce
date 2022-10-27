using Microsoft.AspNetCore.Mvc;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Core.Interfaces;
using Shopping.Core.Entities;
using AutoMapper;


namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {

        #region Fields and CTOR

        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
                                      IProductRepository productRepository,
                                      ILoggerManager logger,
                                      IMapper mapper)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        #endregion


        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
        {

            var cartItem = await shoppingCartRepository.GetCartItem(id);

            if (cartItem == null)
            {
                logger.LogError("GetCartItem; could not get cartItem");
                return NotFound();
            }

            var product = await productRepository.GetProduct(cartItem.ProductId);

            if (product == null)
            {
                logger.LogError("GetCartItem; could not get product");
                return NotFound();
            }

            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);

        }

        [HttpGet]
        [Route("{userStringId}/GetCartItems2")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems2(string userStringId)
        {

            var cartItems = await shoppingCartRepository.GetCartItems2(userStringId);

            if (cartItems == null)
            {
                logger.LogError("GetCartItems; could not get cartItems");
                return NoContent();
            }

            var products = await productRepository.GetProducts();

            if (products == null)
            {
                logger.LogError("GetCartItems; could not get products from system");
                throw new Exception("No products exist in the system");
            }

            var cartItemsDto = cartItems.ConvertToDto(products);

            return Ok(cartItemsDto);

        }


        [HttpPost]
        public async Task<ActionResult<CartItemDto>> AddCartItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {

            var newCartItem = await shoppingCartRepository.AddCartItem(cartItemToAddDto);

            if (newCartItem == null)
            {
                logger.LogError("AddCartItem; could not get new cartItem");
                return NoContent();
            }

            var product = await productRepository.GetProduct(newCartItem.ProductId);

            if (product == null)
            {
                logger.LogError("AddCartItem; could not get product from system");
                throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
            }

            var newCartItemDto = newCartItem.ConvertToDto(product);

            return CreatedAtAction(nameof(GetCartItem), new { id = newCartItemDto.Id }, newCartItemDto);

        }



        [HttpPatch("UpdateCartItem/{id}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int id, ShoppingCartItem shoppingCartItem)
        {

            var cartItem = await shoppingCartRepository.UpdateCartItem(id, shoppingCartItem);

            if (cartItem == null)
            {
                logger.LogError("Update CartItem; could not find cartItem");
                return NotFound();
            }

            mapper.Map(shoppingCartItem, cartItem);

            await shoppingCartRepository.UpdateCartItem(id, cartItem);

            return NoContent();



        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteCartItem(int id)
        {

            var cartItem = await shoppingCartRepository.DeleteCartItem(id);

            if (cartItem == null)
            {
                logger.LogError("DeleteCartItem; could not get cartItem");
                return NotFound();
            }

            var product = await productRepository.GetProduct(cartItem.ProductId);

            if (product == null)
            {
                return NotFound();

            }


            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);

        }




        [HttpPost("AddCart")]
        public async Task<ActionResult<ShoppingCart>> AddCart([FromBody] ShoppingCart shoppingCart)
        {

            var newCart = await shoppingCartRepository.AddCart(shoppingCart);

            if (newCart == null)
            {
                logger.LogError("AddCart; could not get new cart");
                return NoContent();
            }

            var cart = await shoppingCartRepository.GetShoppingCart(newCart.CartStringId);

            if (cart == null)
            {
                logger.LogError("AddCart; could not get new cart from system");
                throw new Exception($"Something went wrong when attempting to retrieve crt (cartId:({shoppingCart.CartStringId})");
            }


            // needs to return right type of Id/cartStringId
            //return CreatedAtAction(nameof(GetShoppingCart), new { id = shoppingCart.Id }, cart);

            return Ok(cart);

        }


        [HttpGet("{cartStringId}/GetShoppingCart")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string cartStringId)
        {

            var cart = await shoppingCartRepository.GetShoppingCart(cartStringId);

            if (cart == null)
            {
                logger.LogError("GetShoppingCart; could not get cart");
                return NotFound();
            }

            return Ok(cart);

        }


        [HttpDelete("{cartStringId}/deleteCart")]
        public async Task<ActionResult<ShoppingCart>> DeleteShoppingCart(string cartStringId)
        {

            var cartToDelete = await shoppingCartRepository.GetShoppingCart(cartStringId);

            if (cartToDelete == null)
            {
                logger.LogError("Delete ShoppingCart; could not find cart to delete");
                return NotFound($"Cart with Id = {cartStringId} not found");
            }

            var deletedCart = await shoppingCartRepository.DeleteShoppingCart(cartStringId);

            return Ok(deletedCart);

        }



        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {

            var cartItem = await shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);

            if (cartItem == null)
            {
                logger.LogError("UpdateQty; could not get cartItem");
                return NotFound();
            }


            var product = await productRepository.GetProduct(cartItem.ProductId);

            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);

        }


   


        //Not used right now
        [HttpGet]
        [Route("{userId}/GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(string userId)
        {

            var cartItems = await shoppingCartRepository.GetCartItems(userId);

            if (cartItems == null)
            {
                logger.LogError("GetCartItems; could not get cartItems");
                return NoContent();
            }

            var products = await productRepository.GetProducts();

            if (products == null)
            {
                logger.LogError("GetCartItems; could not get products from system");
                throw new Exception("No products exist in the system");
            }

            var cartItemsDto = cartItems.ConvertToDto(products);

            return Ok(cartItemsDto);

        }


    }
}

