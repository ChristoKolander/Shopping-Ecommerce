using Shopping.Core.Entities;
using System.Linq;

namespace Shopping.Infrastructure.Data.Repositories
{
    public static class ProductRepositoryExtensions
    {

        //Not used. Did not get it to work in client!

        public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm=null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return products.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

    }
}
