using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }
        IShoppingCartRepository CartItem { get; }
        Task SaveAsync();

    }
}
