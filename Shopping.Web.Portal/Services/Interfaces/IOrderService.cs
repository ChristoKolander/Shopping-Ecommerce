using Shopping.Core.Entities.CQRSresponses;
using Shopping.Shared;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IOrderService
    {
        Task<RequestResponse> AddOrder(OrderResponse Order);

    }
}
