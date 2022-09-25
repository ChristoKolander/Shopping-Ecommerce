using Shopping.Models.Dtos.CRUDs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Services.Interfaces
{
    public interface IShoppingCartService
    {

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);

        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

    }
}
