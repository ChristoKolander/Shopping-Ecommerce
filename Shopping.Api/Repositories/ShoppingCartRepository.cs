 using Microsoft.EntityFrameworkCore;
using Shopping.Api.Data;
using Shopping.Api.Entities;
using Shopping.Api.Repositories.Interfaces;
using Shopping.Models.Dtos.CRUDs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<CartItem>, IShoppingCartRepository
    {

        private readonly ShoppingDbContext shoppingDbContext;

        public ShoppingCartRepository(ShoppingDbContext shoppingDbContext)
            : base(shoppingDbContext)
        {
            this.shoppingDbContext = shoppingDbContext;
        }
    
     
        public async Task<IEnumerable<CartItem>> GetCartItems(int userId)
        {
            return await(from cart in shoppingDbContext.Carts
                         join cartItem in shoppingDbContext.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.UserId == userId
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             Qty = cartItem.Qty,
                             CartId = cartItem.CartId
                         }).ToListAsync();
        }

        public async Task<CartItem> GetCartItem(int id)
        {

            return await(from cart in shoppingDbContext.Carts
                         join cartItem in shoppingDbContext.CartItems
                         on cart.Id equals cartItem.CartId
                         where cartItem.Id == id
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             Qty = cartItem.Qty,
                             CartId = cartItem.CartId
                         }).SingleOrDefaultAsync();
        }

        
      
        public async Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto)
        {          

            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await(from product in shoppingDbContext.Products
                                 where product.Id == cartItemToAddDto.ProductId
                                 select new CartItem
                                 {
                                     CartId = cartItemToAddDto.CartId,
                                     ProductId = product.Id,
                                     Qty = cartItemToAddDto.Qty
                                 }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await shoppingDbContext.CartItems.AddAsync(item);
                    await shoppingDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;

        }
     
        public async Task<CartItem> DeleteCartItem(int id)
        {
            var item = await shoppingDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                shoppingDbContext.CartItems.Remove(item);
                await shoppingDbContext.SaveChangesAsync();
            }

            return item;

        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await shoppingDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await shoppingDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await shoppingDbContext.CartItems.AnyAsync(c =>
                        c.CartId == cartId &&
                        c.ProductId == productId);
        }


    }
}
