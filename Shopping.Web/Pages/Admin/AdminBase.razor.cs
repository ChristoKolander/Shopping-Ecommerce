using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Shopping.Web.Pages.Admin
{
    public class AdminBase: ComponentBase
    {
        public string Id { get; set; }

        public AdminBase()
        {

        }

        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckRoleMemberShip = authState.User.IsInRole("Administrator") || authState.User.IsInRole("Manager");

            if (CheckRoleMemberShip == true)
            {
                StateHasChanged();
            }

        }
    }
}
