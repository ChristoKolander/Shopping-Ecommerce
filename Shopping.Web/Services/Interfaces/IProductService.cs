using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Features.PagingFeatures;
using Shopping.Web.Features.RequestFeatures;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProduct(int id);
        Task<PagingResponse<ProductDto>> GetProducts(QueryStringParameters queryStringParameters = null);
        
        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
        Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId);

        Task<ProductDto> DeleteProduct(int id);
        Task<ProductDto> UpdateProduct(ProductDto productDto);
        Task<ProductCreateDto> CreateProduct(ProductCreateDto productCreateDto);

        Task<string> UploadProductImage(MultipartFormDataContent content);
    }

}

