using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


namespace Shopping.Web.Portal.Pages.Admin
{
    public class AdminBase: ComponentBase
    {
        public string Id { get; set; }

        public AdminBase()
        {
        }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckRoleMemberShip = authState.User.IsInRole("Administrators") || authState.User.IsInRole("Managers");

            if (CheckRoleMemberShip == true)
            {
                StateHasChanged();
            }

        }
    }
}
