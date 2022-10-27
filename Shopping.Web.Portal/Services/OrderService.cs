using Shopping.Core.Entities.CQRSresponses;
using Shopping.Web.Portal.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shopping.Web.Portal.Services
{
    public class OrderService : IOrderService
    {

        # region Fields and CTOR

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;

        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        # endregion

        public async Task<RequestResponse> AddOrder(OrderResponse order)
        {


            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/orders/order", order);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(RequestResponse);
                }

                return await response.Content.ReadFromJsonAsync<RequestResponse>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }

    }
}


      