using Shopping.Core.Entities;
using Shopping.Shared.Entities.RequestFeatures;
using Shopping.Core.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
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
        Task<PagedList<Product>> GetProductsFilteredByPrice(ProductParameters productParameters = null);


        Task<IEnumerable<Product>> Search(
            Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null,
            string includeProperties = "");


        //Task<PagedList<Product>> Search(SearchParameters searchParameters);


    }
}
