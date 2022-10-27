using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages
{
    public class EditProductBase: ComponentBase 
    {
        [Parameter]
        public int Id { get; set; } = 0;

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
 

        public ProductUpdateDto ProductUpdateDto { get; set; } = new ProductUpdateDto();

        protected Shopping.Web.Portal.Components.ConfirmBase DeleteConfirmation { get; set; }

       
        [Inject]
        public IProductService productService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

  
        protected async override Task OnInitializedAsync()
        {
           
            ProductUpdateDto = await productService.GetProduct(Id);
        }

        public async Task Update()
        {
            
                await productService.UpdateProduct(ProductUpdateDto);

                if (ProductUpdateDto.Id > 0)
                {
                    IsVisible = true;
                    RecordName = ProductUpdateDto.Name;
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
                
             ProductUpdateDto Success =   await productService.DeleteProduct(ProductUpdateDto.Id);

             if (Success != null)

                IsVisible = true;
                RecordName = ProductUpdateDto.Name;
                Result = true;
                Delete = true;
            }

        }
    }
}


