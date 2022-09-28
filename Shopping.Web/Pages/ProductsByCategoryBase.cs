using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shopping.Web.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {

        [Parameter]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        protected override async Task OnParametersSetAsync()
        {

            Products = await ProductService.GetItemsByCategory(CategoryId);

            if (Products != null && Products.Count() > 0)
            {
                ProductDto productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);

                if (productDto != null)
                {
                    CategoryName = productDto.CategoryName;
                }

            }

        }
        
    }
}
