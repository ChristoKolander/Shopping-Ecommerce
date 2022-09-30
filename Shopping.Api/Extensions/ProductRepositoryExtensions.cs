using Shopping.Api.Entities;
using System.Linq;

namespace Shopping.Api.Extensions
{
    public static class ProductRepositoryExtensions
    {
        public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm=null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return products.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

    }
}
