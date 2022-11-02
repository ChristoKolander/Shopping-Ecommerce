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

        
        Task<CartItemDto> UpdateCartItem(CartItemDto cartItemDto);
        Task<List<CartItemDto>> GetCartItems2(string userClaimId);
        Task<CartItemDto> AddCartItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteCartItem(int Id);
    
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

        Task<List<CartItemDto>> GetCartItems(string userStringId);
        Task<CartItemDto> ReplaceCartItems(CartItemDto cartItemDto);
        Task<List<ShoppingCartItem>> DeleteCartItems(string cartStringId);


        Task<ShoppingCart> GetShoppingCart (string cartStringId);
        Task<ShoppingCart> CreateShoppingCart(ShoppingCart shoppingCart);    
        Task<ShoppingCart> DeleteShoppingCart(string cartStringId);

    }
}
