using Shopping.Api.Entities;
using Shopping.Api.Entities.RequestFeatures;
using Shopping.Models.Dtos.CRUDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Repositories.Interfaces
{
    public interface IShoppingCartRepository: IRepositoryBase<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItems(int userId);
        Task<CartItem> GetCartItem(int id);
        Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<CartItem> DeleteCartItem(int id);
       
        
    }
}
