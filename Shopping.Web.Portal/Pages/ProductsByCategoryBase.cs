using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages
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
                ProductDto productDto = Products.FirstOrDefault(p => p.ProductCategoryId == CategoryId);

                if (productDto != null)
                {
                    CategoryName = productDto.ProductCategoryName;
                }

            }

        }
        
    }
}
