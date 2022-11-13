using Shopping.Web.Portal.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;
using Shopping.Web.Portal.Features.PagingFeatures;
using Shopping.Web.Portal.Features.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.CRUDs;

namespace Shopping.Web.Portal.Services
{
    public class ProductService : IProductService
    {

        #region Fields and CTOR

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
        }

        #endregion


        public async Task<ProductUpdateDto> GetProduct(int id)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(ProductUpdateDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductUpdateDto>();
            }
            else
            {              
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
        }     
        
        public async Task<PagingResponse<ProductDto>> GetProducts(QueryStringParameters queryStringParameters)
        {
           
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = queryStringParameters.PageNumber.ToString()
                //["searchTerm"] = queryStringParameters.SearchTerm == null ? "" : queryStringParameters.SearchTerm

            };

            HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString("api/product/allProducts", queryStringParam));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var pagingResponse = new PagingResponse<ProductDto>
            {
                Items = System.Text.Json.JsonSerializer.Deserialize<List<ProductDto>>(content, options),
                MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), options)
            };


            return pagingResponse;

        }

        public async Task<ProductUpdateDto> UpdateProduct(ProductUpdateDto productUpdateDto)
        {

            var jsonRequest = JsonConvert.SerializeObject(productUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/product/{productUpdateDto.Id}", content);


            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(patchContent);
            }

            return productUpdateDto;

        }

        public async Task<ProductCreateDto> CreateProduct(ProductCreateDto productCreateDto)
        {


            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/product", productCreateDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<ProductCreateDto>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }

        public async Task<ProductUpdateDto> DeleteProduct(int id)
        {

            HttpResponseMessage response = await httpClient.DeleteAsync($"api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductUpdateDto>();

            }

            return default(ProductUpdateDto);
        }


        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            
            var response = await httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }
            else
            {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
        }            
       
        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
         
            HttpResponseMessage response = await httpClient.GetAsync("api/Product/GetProductCategories");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductCategoryDto>();
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code - {response.StatusCode} Message - {message}");
            }
        }
                  

        public async Task<string> UploadProductImage(MultipartFormDataContent content)
        {
            var postResult = await httpClient.PostAsync("api/upload", content);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {
                var imgUrl = Path.Combine("https://localhost:7200/", postContent);
                return imgUrl;
            }
        }

    }
}
