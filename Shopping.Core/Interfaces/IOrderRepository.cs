using Shopping.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(int basketId, Address shippingAddress);

    }
}
