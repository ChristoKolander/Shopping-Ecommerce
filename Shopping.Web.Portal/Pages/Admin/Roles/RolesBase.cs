using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;

namespace Shopping.Web.Portal.Pages.Admin.Roles
{
    public class RolesBase: ComponentBase
    {
        public string Id { get; set; }
        public string ErrorMessage { get; set; }

        public RoleDto Role { get; set; } = new RoleDto();
        public List<RoleDto> RoleDtoList { get; set; } = new List<RoleDto>();

      
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

        [Inject]
        public IAdministrationService administrationService { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

       
        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckAdminPolicy = await AuthService.AuthorizeAsync(authState.User, "AdminRolePolicy");

            if (CheckAdminPolicy.Succeeded)
            {
                await GetRoles();
            }
        }

        private async Task GetRoles()
        {
            RoleDtoList = await administrationService.GetRoles();
        }

        public void Cancel_Click()
        {
            NavigationManager.NavigateTo("admin/users");


        }

        
    }
}
