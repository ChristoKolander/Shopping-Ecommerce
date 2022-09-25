using Microsoft.EntityFrameworkCore;
using Shopping.Api.Data;
using Shopping.Api.Entities;
using Shopping.Api.Entities.RequestFeatures;
using Shopping.Api.Paging;
using Shopping.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Api.Extensions;
using Shopping.Models.Dtos.CRUDs;

namespace Shopping.Api.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository

    {
        #region Fields and CTOR
        private readonly ShoppingDbContext shoppingDbContext;

        public ProductRepository(ShoppingDbContext shoppingDbContext)
            : base(shoppingDbContext)
        {

            this.shoppingDbContext = shoppingDbContext;
        }
        #endregion

        public async Task<Product> GetProduct(int id)
        {
            return await FindByCondition(p => p.Id == id)
                                    .Include(p => p.ProductCategory)
                                    .SingleOrDefaultAsync(p => p.Id == id);

        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
           return await FindAll()
                       .Include(p => p.ProductCategory)
                       .OrderBy(p => p.Id)
                       .ToListAsync();

        }


        public async Task<PagedList<Product>> GetProductsWithParams(QueryStringParameters queryStringParameters)
        {
            return await PagedList<Product>.ToPagedList(FindAll()
                                           .Search(queryStringParameters.SearchTerm)
                                           .Include(p => p.ProductCategory)
                                           .OrderBy(p => p.Id),
                                            queryStringParameters.PageNumber,
                                            queryStringParameters.PageSize);

        }


        public async Task<PagedList<Product>> GetProductsFiltered(ProductParameters productParameters)
        {


            var products = FindByCondition(p => p.Price >= productParameters.MinPrice &&
                                                p.Price <= productParameters.MaxPrice)
                                  .Include(p => p.ProductCategory)
                                  .OrderBy(p => p.Price);

            return await PagedList<Product>.ToPagedList(
                          products,
                          productParameters.PageNumber,
                          productParameters.PageSize);
        }


        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await shoppingDbContext.ProductCategories.ToListAsync();

            return categories;
        }


        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await shoppingDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }


        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {

            var products = await FindAll()
                                .Include(p => p.ProductCategory)
                                .Where(p => p.CategoryId == id).ToListAsync();

            return products;

        }


        public async Task<Product> CreateProduct(Product product)
        {
            var result = await shoppingDbContext.Products.AddAsync(product);
            await shoppingDbContext.SaveChangesAsync();
            return result.Entity;
        }


        public async Task<Product> UpdateProduct(Product product)
        {
            var prod = await shoppingDbContext.Products
                                .FirstOrDefaultAsync(p => p.Id == product.Id);


            if (prod != null)
            {
                prod.Name = product.Name;
                prod.Description = product.Description;
                prod.ImageURL = product.ImageURL;
                prod.Price = product.Price;
                prod.CategoryId = product.CategoryId;
                prod.ProductCategory = product.ProductCategory;
                prod.Qty = product.Qty;
                prod.WebApiAdminInfoOnly = product.WebApiAdminInfoOnly;

        await shoppingDbContext.SaveChangesAsync();

                return prod;
            }

            return null;
        }


        public async Task<Product> DeleteProduct(int productId)
        {
            var product = await shoppingDbContext.Products
                         .FirstOrDefaultAsync(p => p.Id== productId);
            if (product != null)
            {
                shoppingDbContext.Products.Remove(product);
                await shoppingDbContext.SaveChangesAsync();
                return product;
            }
            return null;
        }


        //public async Task<PagedList<Product>> Search(SearchParameters searchParameters)
        //{
        //    var products = await FindAll()
        //        .Search(searchParameters.SearchTerm)
        //        .Include(p => p.ProductCategory)
        //        .ToListAsync();

        //    return await PagedList<Product>
        //        .ToPagedList(products, searchParameters.PageNumber, searchParameters.PageSize);
        //}
    }


}
