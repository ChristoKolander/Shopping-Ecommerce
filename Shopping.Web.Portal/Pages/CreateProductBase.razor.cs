using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Web.Portal.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Shopping.Web.Portal.Pages
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

        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckAdminPolicy = await AuthService.AuthorizeAsync(authState.User, "AdminRolePolicy");


            if (CheckAdminPolicy.Succeeded)
            {
                StateHasChanged();
            }
        }
        

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

        public void AssignImageUrl(string imgUrl) => productCreateDto.ImageURL = imgUrl;

    }
}

