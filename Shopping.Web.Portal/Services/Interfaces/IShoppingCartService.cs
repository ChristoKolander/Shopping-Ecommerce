using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IShoppingCartService
    {

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);

        Task<List<CartItemDto>> GetItems(string userStringId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int Id);
        Task <List<ShoppingCartItem>> DeleteItems(string cartStringId);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

        Task<CartItemDto> ReplaceItems(CartItemDto cartItemDto);

        Task<CartItemDto> UpdateCartItem(CartItemDto cartItemDto);
    
        Task<ShoppingCart> GetShoppingCart (string cartStringId);
        Task<ShoppingCart> CreateShoppingCart(ShoppingCart shoppingCart);
        Task<List<CartItemDto>> GetItems2(string userClaimId); 
        Task<ShoppingCart> DeleteShoppingCart(string cartStringId);

    }
}
