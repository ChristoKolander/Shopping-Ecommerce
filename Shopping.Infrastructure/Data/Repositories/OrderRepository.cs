using Shopping.Core.Entities;
using Shopping.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {

        private readonly ProductContext productContext;

        public OrderRepository(ProductContext productContext) 
                : base(productContext)
        {
            this.productContext = productContext;
        }

        public Task CreateOrderAsync(int basketId, Address shippingAddress)
        {
            throw new NotImplementedException();
        }
    }
}
