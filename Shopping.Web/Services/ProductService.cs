using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shopping.Web.Features.PagingFeatures;
using Shopping.Web.Features.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace Shopping.Web.Services
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


        public async Task<ProductDto> GetProduct(int id)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/V1/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
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

            HttpResponseMessage response = await httpClient.GetAsync(QueryHelpers.AddQueryString("api/V1/product", queryStringParam));
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
   
        
        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            
            var response = await httpClient.GetAsync($"api/V1/Product/{categoryId}/GetItemsByCategory");

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
         
            HttpResponseMessage response = await httpClient.GetAsync("api/V1/Product/GetProductCategories");

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
            
      
        public async Task<ProductDto> UpdateProduct(ProductDto productDto)
        {

            var jsonRequest = JsonConvert.SerializeObject(productDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/V1/product/{productDto.Id}", content);

                
            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {                   
                throw new ApplicationException(patchContent);
            }

            return productDto;

        }        
        public async Task<ProductCreateDto> CreateProduct(ProductCreateDto productCreateDto)
        {

            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/V1/product", productCreateDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductCreateDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductCreateDto>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status:{response.StatusCode} Message -{message}");
            }

        }          
        public async Task<ProductDto> DeleteProduct(int id)
        {

            HttpResponseMessage response = await httpClient.DeleteAsync($"api/v1/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }

            return default(ProductDto);
        }
          
       
        public async Task<string> UploadProductImage(MultipartFormDataContent content)
        {
            var postResult = await httpClient.PostAsync("UploadFiles", content);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {
                var imgUrl = Path.Combine("api /V1/UploadFiles", postContent);
                return imgUrl;
            }
        }

    }
}
