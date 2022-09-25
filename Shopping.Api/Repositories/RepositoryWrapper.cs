using Shopping.Api.Data;
using Shopping.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper

    {
        private ShoppingDbContext repoContext;
        private IProductRepository product;
        private IShoppingCartRepository shoppingCart;
        
        public IProductRepository Product
        {
            get
            {
                if (product == null)
                {
                    product = new ProductRepository(repoContext);
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
                    shoppingCart = new ShoppingCartRepository(repoContext);
                }
                return shoppingCart;
            }
        }


        public RepositoryWrapper(ShoppingDbContext repositoryContext)
        {
            repoContext = repositoryContext;
        }

        public async Task SaveAsync()
        {
            await repoContext.SaveChangesAsync();
        }

      


    }
}
    

