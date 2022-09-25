using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Models.Dtos.CRUDs;

namespace Shopping.Web.Services.Interfaces
{
    public interface IManageCartItemsLocalStorageService
    {
        Task RemoveCollection();
        Task<List<CartItemDto>> GetCollection();
        Task SaveCollection(List<CartItemDto> cartItemDtos);
      

    }
}
