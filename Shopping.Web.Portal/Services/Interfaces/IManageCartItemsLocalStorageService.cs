using Shopping.Shared.Dtos.CRUDs;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IManageCartItemsLocalStorageService
    {
        Task RemoveCollection();
        Task<List<CartItemDto>> GetCollection();
        Task SaveCollection(List<CartItemDto> cartItemDtos);
     
        Task RemoveUserClaimStringId();
        Task<string> GetUserClaimStringId();
        Task<string> AddUserClaimStringId();
        Task SaveUserClaimStringId(string UserClaimStringId);
        
        Task SaveCartStringId(string cartStringId);
        Task<string> GetCartStringId();
        Task RemoveCartStringId();

        Task AddCartCreatedValue(string CartCreatedValue);
        Task<string> GetCartCreatedValue();
    }
}
