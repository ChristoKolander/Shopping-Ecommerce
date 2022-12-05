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
    public class ShoppingCartService : ICartService
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

        #endregion

        # region CartItems

        public async Task<CartItemDto> AddCartItem(CartItemToAddDto cartItemToAddDto)
        {

            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/Cart", cartItemToAddDto);

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

        public async Task<CartItemDto> UpdateCartItem(CartItemDto cartItemDto)
        {
            var jsonRequest = JsonConvert.SerializeObject(cartItemDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/cart/updateCartItem/{cartItemDto.Id}", content);

            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(patchContent);


            }

            return cartItemDto;

        }

        public async Task<CartItemDto> DeleteCartItem(int Id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Cart/{Id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return default(CartItemDto);
        }

        public async Task<List<CartItemDto>> GetCartItems(string userClaimId)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/Cart/{userClaimId}/GetCartItems");

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

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/Cart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return null;

        }

        #endregion

        # region Cart


        public async Task<Cart> CreateCart(Cart cart)
        {
      
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Cart/AddCart", cart);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(Cart);
                }

               
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Cart>(content);

                return result;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
        }
        
        public async Task<Cart> DeleteCart(string cartStringId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Cart/{cartStringId}/deleteCart");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Cart>();
            }
            return default(Cart);
        }


        # endregion

        #region NotUsedRightNow

        //Not used at the moment, predesessor...keeping in case need something similar..
        public async Task<List<CartItemDto>> GetCartItems2(string userStringId)
        {


            HttpResponseMessage response = await httpClient.GetAsync($"api/Cart/{userStringId}/GetCartItems");

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
        public async Task<List<CartItem>> DeleteCartItems(string cartStringId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Cart/{cartStringId}/deleteCartItems");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CartItem>>();
            }

            return default(List<CartItem>);
        }
        public async Task<CartItemDto> ReplaceCartItems(CartItemDto cartItemDto)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<CartItemDto>("api/Cart/ReplaceItems", cartItemDto);

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

        public async Task<Cart> GetCart(string cartStringId)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/Cart/{cartStringId}/GetCart");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(Cart);
                }

                return await response.Content.ReadFromJsonAsync<Cart>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }





        # endregion
    }

}   







        


        
    

