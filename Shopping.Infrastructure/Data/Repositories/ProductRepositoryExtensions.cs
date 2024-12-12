using Shopping.Core.Entities;
using System.Linq;

namespace Shopping.Infrastructure.Data.Repositories
{
    public static class ProductRepositoryExtensions
    {

        // This Search Extension Method is used when landing on the "home page" AND in conjuction with
        // Pagination -> QueryParameter -> MetaData. As for now, don't remember how it works. Checking it out later.

        public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm=null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return products.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

    }
}
