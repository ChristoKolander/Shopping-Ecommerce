using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{


    // This Interface is not used at the moment. Just added complexity when not needed.

    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }
        ICartRepository CartItem { get; }
        Task SaveAsync();

    }
        
}
