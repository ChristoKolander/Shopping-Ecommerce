using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface ICartService
    {

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);

        
        Task<CartItemDto> UpdateCartItem(CartItemDto cartItemDto);
        Task<List<CartItemDto>> GetCartItems(string userClaimStringId);
        Task<CartItemDto> AddCartItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteCartItem(int Id);
    
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

       
        Task<CartItemDto> ReplaceCartItems(CartItemDto cartItemDto);
        Task<List<CartItem>> DeleteCartItems(string cartStringId);


        Task<Cart> GetCart (string cartStringId);
        Task<Cart> CreateCart(Cart cart);    
        Task<Cart> DeleteCart(string cartStringId);


        //Task<List<CartItemDto>> GetCartItems2(string userClaimId);
         //Task<List<CartItemDto>> GetCartItems(string userStringId);
    }
}
