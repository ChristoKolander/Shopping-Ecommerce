using Microsoft.EntityFrameworkCore;
using Shopping.Core.Entities;
using Shopping.Shared.Entities.RequestFeatures;
using Shopping.Core.Interfaces;
using Shopping.Core.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Shopping.Infrastructure.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {   
        private readonly ProductContext productContext;

        public ProductRepository(ProductContext ProductContext)
            : base(ProductContext)
        {
            productContext = ProductContext;
        }
    
        public async Task<Product> GetProduct(int id)
        {
            return await FindByCondition(p => p.Id == id)
                                    .Include(p => p.ProductCategory)
                                    .FirstOrDefaultAsync(p => p.Id == id);

        }
        
        public async Task<IEnumerable<Product>> GetProducts()
        {
            
            return await FindAllAsync()
                       .Include(p => p.ProductCategory)
                       .OrderBy(p => p.Id)
                       .ToListAsync();

        }


        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {

            var products = await FindAll()
                                .Include(p => p.ProductCategory)
                                .Where(p => p.ProductCategoryId == id).ToListAsync();

            return products;

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

        public async Task<PagedList<Product>> GetProductsFilteredByPrice(ProductParameters productParameters)
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


        public async Task<Product> UpdateProduct(Product product)
        {
            var prod = await productContext.Products
                                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (prod != null)
            {
                prod.Name = product.Name;
                prod.Description = product.Description;
                prod.ImageURL = product.ImageURL;
                prod.Price = product.Price;
                prod.Qty = product.Qty; 
                prod.ProductCategoryId = product.ProductCategoryId;
                prod.ProductCategory = product.ProductCategory;
                //prod.Qty = product.Qty;

                await productContext.SaveChangesAsync();

                return prod;
            }

            return null;
        }
        
        public async Task<Product> CreateProduct(Product product)
        {
            var result = await productContext.Products.AddAsync(product);
            await productContext.SaveChangesAsync();
            return result.Entity;
        }
 
        public async Task<Product> DeleteProduct(int productId)
        {
            var product = await productContext.Products
                         .FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                productContext.Products.Remove(product);
                await productContext.SaveChangesAsync();
                return product;
            }
            return null;
        }

       
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await productContext.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await productContext.ProductCategories.FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }


        public async Task<IEnumerable<Product>> Search(Expression<Func<Product, bool>> filter = null, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null, string includeProperties = "")
        {
                    
                IQueryable<Product> query = productcontext.Set<Product>();

                // Apply the filter
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Include the specified properties
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                // Sort
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return await query.ToListAsync();
                }
            
          
        }


        #region NotUsedRightNow

        //public async Task<IEnumerable<Product>> GetProducts()
        //{
        //   return await FindAll()
        //               .Include(p => p.ProductCategory)
        //               .OrderBy(p => p.Id)
        //               .ToListAsync();

        //}


        //public async Task<PagedList<Product>> Search(SearchParameters searchParameters)
        //{
        //    var products = await FindAll()
        //        .Search(searchParameters.SearchTerm)
        //        .Include(p => p.ProductCategory)
        //        .ToListAsync();

        //    return await PagedList<Product>
        //        .ToPagedList(products, searchParameters.PageNumber, searchParameters.PageSize);
        //}


        # endregion

    }


}
