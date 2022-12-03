using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Core.Interfaces;
using Shopping.Shared.Dtos.CRUDs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shopping.Infrastructure.Data.Repositories
{
    public class CartRepository : RepositoryBase<CartItem>, ICartRepository
    {

        private readonly ProductContext productContext;

        public CartRepository(ProductContext ProductContext)
            : base(ProductContext)
        {
            productContext = ProductContext;
        }


        public async Task<CartItem> GetCartItem(int id)
        {

            return await (from cart in productContext.Carts
                          join cartItem in productContext.CartItems
                          on cart.UserClaimStringId equals cartItem.UserClaimStringId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              UserClaimStringId = cart.UserClaimStringId

                          }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(string userStringId)
        {
            return await (from cart in productContext.Carts
                          join cartItem in productContext.CartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cart.UserClaimStringId == userStringId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              Price = cartItem.Price,   

                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateCartItem(int id, CartItem CartItem)
        {
            var cartItem = await productContext.CartItems
                                .FirstOrDefaultAsync(ci => ci.Id == CartItem.Id);

            if (cartItem != null)
            {

                cartItem.CartStringId = cartItem.CartStringId;
                await productContext.SaveChangesAsync();

                return cartItem;
            }

            return null;
        }


        public async Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto)
        {

            if (await CartItemExists(cartItemToAddDto.CartStringId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in productContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartStringId = cartItemToAddDto.CartStringId,
                                      UserClaimStringId = cartItemToAddDto.UserClaimStringId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty

                                  }).FirstOrDefaultAsync();

                if (item != null)
                {
                    var result = await productContext.CartItems.AddAsync(item);
                    await productContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return default;

        }

        private async Task<bool> CartItemExists(string cartStringId, int productId)
        {
            return await productContext.CartItems.AnyAsync(c =>
                        c.CartStringId == cartStringId &&
                        c.ProductId == productId);
        }


        public async Task<CartItem> DeleteCartItem(int id)
        {

            var item = await productContext.CartItems.FindAsync(id);

            if (item != null)
            {
                productContext.CartItems.Remove(item);
                await productContext.SaveChangesAsync();
            }
            // return item? 

            return null;

        }

       

        public async Task<Cart> AddCart(Cart cart)
        {
            var result = await productContext.Carts.AddAsync(cart);
            await productContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Cart> GetCart(string cartStringId)
        {
            var result = await productContext.Carts
                .FirstOrDefaultAsync(c => c.CartStringId == cartStringId);   
            return result;
        }

        public async Task<Cart> DeleteCart(string cartStringId)
        {
            var cart = await productContext.Carts
                         .FirstOrDefaultAsync(sc => sc.CartStringId == cartStringId);
            if (cart != null)
            {
                productContext.Carts.Remove(cart);
                await productContext.SaveChangesAsync();
                return cart;
            }
            return null;
        }

        
        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await productContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await productContext.SaveChangesAsync();
                return item;
            }

            return null;
        }


        # region NotUsedRightNow
        //Not used right now.
        public async Task<IEnumerable<CartItem>> GetCartItems2(string userStringId)
        {
            return await (from cart in productContext.Carts
                          join cartItem in productContext.CartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cart.UserClaimStringId == userStringId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty

                          }).ToListAsync();
        }

        public async Task<CartItem> GetCartItem2(int id)
        {

            return await (from cart in productContext.Carts
                          join cartItem in productContext.CartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              UserClaimStringId = cartItem.UserClaimStringId

                          }).SingleOrDefaultAsync();
        }

        # endregion

    }
}

