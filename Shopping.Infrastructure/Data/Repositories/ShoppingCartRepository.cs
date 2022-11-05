using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Core.Interfaces;
using Shopping.Shared.Dtos.CRUDs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shopping.Infrastructure.Data.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCartItem>, IShoppingCartRepository
    {

        private readonly ProductContext productContext;

        public ShoppingCartRepository(ProductContext ProductContext)
            : base(ProductContext)
        {
            productContext = ProductContext;
        }

        public async Task<ShoppingCartItem> GetCartItem(int id)
        {

            return await (from cart in productContext.ShoppingCarts
                          join cartItem in productContext.ShoppingCartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cartItem.Id == id
                          select new ShoppingCartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              UserClaimStringId = cartItem.UserClaimStringId

                          }).SingleOrDefaultAsync();
        }

        public async Task<ShoppingCartItem> GetCartItem2(int id)
        {

            return await (from cart in productContext.ShoppingCarts
                          join cartItem in productContext.ShoppingCartItems
                          on cart.UserClaimStringId equals cartItem.UserClaimStringId
                          where cartItem.Id == id
                          select new ShoppingCartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              UserClaimStringId = cart.UserClaimStringId

                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetCartItems2(string userStringId)
        {
            return await (from cart in productContext.ShoppingCarts
                          join cartItem in productContext.ShoppingCartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cart.UserClaimStringId == userStringId
                          select new ShoppingCartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartStringId = cartItem.CartStringId,
                              Price = cartItem.Price,   

                          }).ToListAsync();
        }

        public async Task<ShoppingCartItem> AddCartItem(CartItemToAddDto cartItemToAddDto)
        {

            if (await CartItemExists(cartItemToAddDto.CartStringId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in productContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new ShoppingCartItem
                                  {
                                      CartStringId = cartItemToAddDto.CartStringId,
                                      UserClaimStringId = cartItemToAddDto.UserClaimStringId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty

                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await productContext.ShoppingCartItems.AddAsync(item);
                    await productContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return default;

        }

        private async Task<bool> CartItemExists(string cartStringId, int productId)
        {
            return await productContext.ShoppingCartItems.AnyAsync(c =>
                        c.CartStringId == cartStringId &&
                        c.ProductId == productId);
        }


        public async Task<ShoppingCartItem> DeleteCartItem(int id)
        {

            var item = await productContext.ShoppingCartItems.FindAsync(id);

            if (item != null)
            {
                productContext.ShoppingCartItems.Remove(item);
                await productContext.SaveChangesAsync();
            }
            // return item? 

            return null;

        }

        public async Task<ShoppingCartItem> UpdateCartItem(int id, ShoppingCartItem shoppingCartItem)
        {
            var cartItem = await productContext.ShoppingCartItems
                                .FirstOrDefaultAsync(sh => sh.Id == shoppingCartItem.Id);

            if (cartItem != null)
            {
             
                cartItem.CartStringId = shoppingCartItem.CartStringId;
                await productContext.SaveChangesAsync();

                return cartItem;
            }

            return null;
        }


        public async Task<ShoppingCart> AddCart(ShoppingCart shoppingCart)
        {
            var result = await productContext.ShoppingCarts.AddAsync(shoppingCart);
            await productContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ShoppingCart> GetShoppingCart(string cartStringId)
        {
            var result = await productContext.ShoppingCarts
                .FirstOrDefaultAsync(c => c.CartStringId == cartStringId);   
            return result;
        }

        public async Task<ShoppingCart> DeleteShoppingCart(string cartStringId)
        {
            var cart = await productContext.ShoppingCarts
                         .FirstOrDefaultAsync(sc => sc.CartStringId == cartStringId);
            if (cart != null)
            {
                productContext.ShoppingCarts.Remove(cart);
                await productContext.SaveChangesAsync();
                return cart;
            }
            return null;
        }

        
        public async Task<ShoppingCartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await productContext.ShoppingCartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await productContext.SaveChangesAsync();
                return item;
            }

            return null;
        }

      

        //Not used right now.
        public async Task<IEnumerable<ShoppingCartItem>> GetCartItems(string userStringId)
        {
            return await (from cart in productContext.ShoppingCarts
                          join cartItem in productContext.ShoppingCartItems
                          on cart.CartStringId equals cartItem.CartStringId
                          where cart.UserClaimStringId == userStringId
                          select new ShoppingCartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty

                          }).ToListAsync();
        }

    }
}

