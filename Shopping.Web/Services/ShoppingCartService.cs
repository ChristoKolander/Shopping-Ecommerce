using Newtonsoft.Json;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {

        public event Action<int> OnShoppingCartChanged;

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }
  
        private readonly HttpClient httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
      
        public async Task<List<CartItemDto>> GetItems(int userId)
        {
           

            HttpResponseMessage response = await httpClient.GetAsync($"api/V1/ShoppingCart/{userId}/GetCartItems");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }                
        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {

            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/V1/ShoppingCart", cartItemToAddDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto);
                }

                return await response.Content.ReadFromJsonAsync<CartItemDto>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }               
        public async Task<CartItemDto> DeleteItem(int id)
        {
            HttpResponseMessage  response = await httpClient.DeleteAsync($"api/V1/ShoppingCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
                return default(CartItemDto);
        }                 
        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {         
            var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/V1/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return null;

        }
                 
    }
}
