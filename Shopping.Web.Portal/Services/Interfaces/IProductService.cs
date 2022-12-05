using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Features.PagingFeatures;
using Shopping.Web.Portal.Features.RequestFeatures;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductUpdateDto> GetProduct(int id);
        Task<PagingResponse<ProductDto>> GetProducts(QueryStringParameters queryStringParameters = null);
        
        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
        Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId);

        Task<ProductUpdateDto> DeleteProduct(int id);
        Task<ProductUpdateDto> UpdateProduct(ProductUpdateDto productUpdateDto);
        Task<ProductCreateDto> CreateProduct(ProductCreateDto productCreateDto);

        Task<string> UploadProductImage(MultipartFormDataContent content);
    }

}

