using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface IShoppingCartRepository : IRepositoryBase<ShoppingCartItem>
    {
        Task<IEnumerable<ShoppingCartItem>> GetCartItems(string userStringId);
        Task<ShoppingCartItem> GetCartItem(int id);
        Task<ShoppingCartItem> AddCartItem(CartItemToAddDto cartItemToAddDto);
        Task<ShoppingCartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<ShoppingCartItem> DeleteCartItem(int id);


        Task<ShoppingCartItem> UpdateCartItem(int id, ShoppingCartItem shoppingCartItem);
        Task<ShoppingCartItem> GetCartItem2(int id);



        Task<IEnumerable<ShoppingCartItem>> GetCartItems2(string userStringId);
        Task<ShoppingCart> AddCart(ShoppingCart shoppingCart);
        Task<ShoppingCart> GetShoppingCart(string cartStringId);

        Task<ShoppingCart> DeleteShoppingCart(string cartStringId);


    }
}
