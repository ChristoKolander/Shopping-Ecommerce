using Microsoft.AspNetCore.Components;
using Shopping.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace Shopping.Web.Auth
{
    public class LogoutBase : ComponentBase
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}