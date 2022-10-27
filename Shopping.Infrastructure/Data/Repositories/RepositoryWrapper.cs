using Shopping.Core.Interfaces;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper

    {
        private ProductContext productContext;
        private IProductRepository product;
        private IShoppingCartRepository shoppingCart;
        
        public IProductRepository Product
        {
            get
            {
                if (product == null)
                {
                    product = new ProductRepository(productContext);
                }
                return product;
            }
        }
        public IShoppingCartRepository CartItem
        {
            get
            {
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCartRepository(productContext);
                }
                return shoppingCart;
            }
        }


        public RepositoryWrapper(ProductContext ProductContext)
        {
            productContext = ProductContext;
        }

        public async Task SaveAsync()
        {
            await productContext.SaveChangesAsync();
        }

      


    }
}
    

