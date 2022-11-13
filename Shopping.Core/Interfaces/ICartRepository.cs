using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface ICartRepository : IRepositoryBase<CartItem>
    {

        Task<CartItem> GetCartItem(int id);
        Task<IEnumerable<CartItem>> GetCartItems(string userStringId);
        Task<CartItem> UpdateCartItem(int id, CartItem cartItem);
        Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto);  
        Task<CartItem> DeleteCartItem(int id);

        Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
      
       
        Task<Cart> AddCart(Cart cart);
        Task<Cart> GetCart(string cartStringId);
        Task<Cart> DeleteCart(string cartStringId);


        //Task<ShoppingCartItem> GetCartItem2(int id);
        //Task<IEnumerable<ShoppingCartItem>> GetCartItems2(string userStringId);

    }
}
