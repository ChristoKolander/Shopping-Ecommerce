using Shopping.Core.Entities;
using Shopping.Core.Entities.RequestFeatures;
using Shopping.Core.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shopping.Core.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {

        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int productId);

        Task<ProductCategory> GetCategory(int id);
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<IEnumerable<Product>> GetProductsByCategory(int id);

        Task<PagedList<Product>> GetProductsWithParams(QueryStringParameters queryStringParameters = null);
        Task<PagedList<Product>> GetProductsFiltered(ProductParameters productParameters = null);

        //Task<PagedList<Product>> Search(SearchParameters searchParameters);




    }
}
