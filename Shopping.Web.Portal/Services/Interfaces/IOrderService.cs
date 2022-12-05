using Shopping.Core.Entities.CQRSresponses;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> GetOrder(int id, string userEmail);
        Task<List<OrderResponse>> GetOrders(string userEmail);
        Task<OrderResponse> UpdateOrder(OrderResponse Order);
        Task<RequestResponse> AddOrder(OrderResponse Order);
        Task<RequestResponse> DeleteOrder(int id);

    }
}
