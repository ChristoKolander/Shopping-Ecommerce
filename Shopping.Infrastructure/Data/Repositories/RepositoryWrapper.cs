using Shopping.Core.Interfaces;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data.Repositories
{

    // This Class is not used at the moment. 


    public class RepositoryWrapper : IRepositoryWrapper

    {
        private ProductContext productContext;
        private IProductRepository product;
        private ICartRepository cart; 
        
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
        public ICartRepository CartItem
        {
            get
            {
                if (cart == null)
                {
                    cart = new CartRepository(productContext);
                }
                return cart;
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
    

