using Newtonsoft.Json;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Web.Portal.Services.Interfaces;
using System.Net.Http.Json;
using System.Text;
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

        #endregion

        public async Task<OrderResponse> GetOrder(int id, string userEmail)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"api/order/order/{id}/{userEmail}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(OrderResponse);
                }

                return await response.Content.ReadFromJsonAsync<OrderResponse>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }

        public async Task<List<OrderResponse>> GetOrders(string userEmail)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"api/order/orders/{userEmail}");
            
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(List<OrderResponse>);
                }

                return await response.Content.ReadFromJsonAsync<List<OrderResponse>>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }

        public async Task<OrderResponse> UpdateOrder(OrderResponse order)
        {

            var jsonRequest = JsonConvert.SerializeObject(order);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync("api/order/order", content);


            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(patchContent);
            }

            return order;

        }

        //     MOVED TO PATCH..
        //    HttpResponseMessage response = await httpClient.PutAsJsonAsync("api/order/order", order);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        //        {
        //            return default(RequestResponse);
        //        }

        //        return await response.Content.ReadFromJsonAsync<RequestResponse>();

        //    }
        //    else
        //    {
        //        var message = await response.Content.ReadAsStringAsync();
        //        throw new Exception($"Http status:{response.StatusCode} Message -{message}");
        //    }

        //}

        public async Task<RequestResponse> AddOrder(OrderResponse order)
        {

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/order", order);

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

        public async Task<RequestResponse> DeleteOrder(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"Order/order/{id}");


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



      