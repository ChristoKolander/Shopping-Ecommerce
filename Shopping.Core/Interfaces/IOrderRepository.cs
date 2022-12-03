using Shopping.Core.Entities;
using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(int basketId, Address shippingAddress);

    }
}
