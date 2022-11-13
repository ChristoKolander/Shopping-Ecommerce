using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }
        ICartRepository CartItem { get; }
        Task SaveAsync();

    }
}
