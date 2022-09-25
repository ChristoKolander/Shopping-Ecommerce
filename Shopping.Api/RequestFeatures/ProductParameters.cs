using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Entities.RequestFeatures
{
    public class ProductParameters: QueryStringParameters
    {
        public uint MinPrice { get; set; } = 1;
        public uint MaxPrice { get; set; } = 50000;
        public bool ValidPriceRange => MaxPrice > MinPrice;

    }
}
