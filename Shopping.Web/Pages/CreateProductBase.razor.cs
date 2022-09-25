using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System.Threading.Tasks;


namespace Shopping.Web.Pages
{
    public class CreateProductBase: ComponentBase
    {

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
      
        public ProductCreateDto productCreateDto = new ProductCreateDto();

        [Inject]
        public IProductService productService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task CreateProduct()
        {
            ProductCreateDto Success;
            
            Success =  await productService.CreateProduct(productCreateDto);

            if (Success.Price != 0)
            {
                IsVisible = true;
                RecordName = productCreateDto.Name;
                Result = true;
                Edit = true;
            }
        }

        public void Cancel_Click()
        {
            NavigationManager.NavigateTo("/");

        }

    }
}

