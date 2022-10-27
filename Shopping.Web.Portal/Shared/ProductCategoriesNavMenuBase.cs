using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Portal.Shared
{
    public class ProductCategoriesNavMenuBase: ComponentBase
    {

        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; }


        protected override async Task OnInitializedAsync()
        {
            
                ProductCategoryDtos = await ProductService.GetProductCategories();
        }
            
    }
}

