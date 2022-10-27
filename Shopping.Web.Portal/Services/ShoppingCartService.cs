using Newtonsoft.Json;
using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Shopping.Web.Portal.Services
{
    public class ShoppingCartService : IShoppingCartService
    {

        # region Fields and CTOR

        public event Action<int> OnShoppingCartChanged;

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        private readonly HttpClient httpClient;
        private JsonSerializerOptions options;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true,
                                                  AllowTrailingCommas = true,
                                                  IncludeFields = true                                                 
                                                  
            };            
        }

        # endregion   

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {

            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);

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
        
        public async Task<CartItemDto> DeleteItem(int Id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/ShoppingCart/{Id}");

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

            HttpResponseMessage response = await httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return null;

        }

        public async Task<List<CartItemDto>> GetItems2(string userClaimId)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/ShoppingCart/{userClaimId}/GetCartItems2");

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

        public async Task<ShoppingCart> GetShoppingCart(string cartStringId)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/ShoppingCart/{cartStringId}/GetShoppingCart");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(ShoppingCart);
                }

                return await response.Content.ReadFromJsonAsync<ShoppingCart>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }
      
        public async Task<ShoppingCart> CreateShoppingCart(ShoppingCart shoppingCart)
        {
      
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/ShoppingCart/AddCart", shoppingCart);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ShoppingCart);
                }

               

          //var result= response.Content.ReadFromJsonAsync<ShoppingCart>(options);


                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ShoppingCart>(content);

                return result;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
        }
        
        public async Task<ShoppingCart> DeleteShoppingCart(string cartStringId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/ShoppingCart/{cartStringId}/deleteCart");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ShoppingCart>();
            }
            return default(ShoppingCart);
        }
    
        public async Task<CartItemDto> UpdateCartItem(CartItemDto cartItemDto)
        {
            var jsonRequest = JsonConvert.SerializeObject(cartItemDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/shoppingcart/updateCartItem/{cartItemDto.Id}", content);

            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(patchContent);


            }

            return cartItemDto;

        }



        //Not used at the moment, predesessor...keeping in case need something similar..
        public async Task<List<CartItemDto>> GetItems(string userStringId)
        {


            HttpResponseMessage response = await httpClient.GetAsync($"api/ShoppingCart/{userStringId}/GetCartItems");

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
        public async Task<List<ShoppingCartItem>> DeleteItems(string cartStringId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/ShoppingCart/{cartStringId}/deleteCartItems");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ShoppingCartItem>>();
            }

            return default(List<ShoppingCartItem>);
        }
        public async Task<CartItemDto> ReplaceItems(CartItemDto cartItemDto)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CartItemDto>("api/ShoppingCart/ReplaceItems", cartItemDto);

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
    }

}   







        


        
    

