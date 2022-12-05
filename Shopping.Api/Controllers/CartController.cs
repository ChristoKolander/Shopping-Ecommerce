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
    public class CartController : ControllerBase
    {

        #region Fields and CTOR

        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;

        public CartController(ICartRepository cartRepository,
                                      IProductRepository productRepository,
                                      ILoggerManager logger,
                                      IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        #endregion

        # region CartItems

        // Used for the Add CartItem Action only.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
        {

            var cartItem = await cartRepository.GetCartItem(id);

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
        [Route("{userStringId}/GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(string userStringId) 
        {

            var cartItems = await cartRepository.GetCartItems(userStringId);

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

            var newCartItem = await cartRepository.AddCartItem(cartItemToAddDto);

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
        public async Task<ActionResult<CartItemDto>> UpdateCartItem(int id, CartItem CartItem)
        {

            var cartItem = await cartRepository.UpdateCartItem(id, CartItem);

            if (cartItem == null)
            {
                logger.LogError("Update CartItem; could not find cartItem");
                return NotFound();
            }

            mapper.Map(CartItem, cartItem);

            await cartRepository.UpdateCartItem(id, cartItem);

            return NoContent();



        }

      
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteCartItem(int id)
        {

            var cartItem = await cartRepository.DeleteCartItem(id);

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

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {

            var cartItem = await cartRepository.UpdateQty(id, cartItemQtyUpdateDto);

            if (cartItem == null)
            {
                logger.LogError("UpdateQty; could not get cartItem");
                return NotFound();
            }


            var product = await productRepository.GetProduct(cartItem.ProductId);

            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);

        }

        #endregion

        #region Carts

        [HttpPost("AddCart")]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart Cart)
        {

            var newCart = await cartRepository.AddCart(Cart);

            if (newCart == null)
            {
                logger.LogError("AddCart; could not get new cart");
                return NoContent();
            }

            var cart = await cartRepository.GetCart(newCart.CartStringId);

            if (cart == null)
            {
                logger.LogError("AddCart; could not get new cart from system");
                throw new Exception($"Something went wrong when attempting to retrieve crt (cartId:({cart.CartStringId})");
            }


            // needs to return right type of Id/cartStringId
            //return CreatedAtAction(nameof(GetShoppingCart), new { id = shoppingCart.Id }, cart);

            return Ok(cart);

        }
      
        
        [HttpDelete("{cartStringId}/deleteCart")]
        public async Task<ActionResult<Cart>> DeleteCart(string cartStringId)
        {

            var cartToDelete = await cartRepository.GetCart(cartStringId);

            if (cartToDelete == null)
            {
                logger.LogError("Delete Cart; could not find cart to delete");
                return NotFound($"Cart with Id = {cartStringId} not found");
            }

            var deletedCart = await cartRepository.DeleteCart(cartStringId);

            return Ok(deletedCart);

        }

        # endregion

    }
}

