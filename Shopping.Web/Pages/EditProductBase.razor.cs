using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.CRUDs;
using Shopping.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace Shopping.Web.Pages
{
    public class EditProductBase: ComponentBase 
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        [Parameter]
        public int Id { get; set; } = 0;

        public ProductDto productDto { get; set; } = new ProductDto();

        protected Shopping.Web.Components.ConfirmBase DeleteConfirmation { get; set; }

        [Inject]
        public IProductService productService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

  
        protected async override Task OnInitializedAsync()
        {
           
            productDto = await productService.GetProduct(Id);
        }

        public async Task Update()
        {
            
                await productService.UpdateProduct(productDto);

                if (productDto.Id > 0)
                {
                    IsVisible = true;
                    RecordName = productDto.Name;
                    Result = true;
                    Edit = true;
                }
        }
                 

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                
                await productService.DeleteProduct(productDto.Id);

                IsVisible = true;
                RecordName = productDto.Name;
                Result = true;
                Delete = true;
            }

        }

        
    }
}
