using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }
        IShoppingCartRepository CartItem { get; }
        Task SaveAsync();

    }
}
