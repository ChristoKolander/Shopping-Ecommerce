using Shopping.Api.Entities;
using Shopping.Api.Entities.RequestFeatures;
using System.Collections.Generic;

using System.Threading.Tasks;
using Shopping.Api.Paging;
using Shopping.Models.Dtos.CRUDs;

namespace Shopping.Api.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {

        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int productId);

        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<PagedList<Product>> GetProductsWithParams(QueryStringParameters queryStringParameters=null);         
        Task<PagedList<Product>> GetProductsFiltered(ProductParameters productParameters = null);

   
        Task<ProductCategory> GetCategory(int id);
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<IEnumerable<Product>> GetProductsByCategory(int id);


        //Task<PagedList<Product>> Search(SearchParameters searchParameters);

      
       

    }
}
