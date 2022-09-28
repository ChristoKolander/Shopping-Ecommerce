using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Pages.Admin.Roles
{
    public class RolesBase: ComponentBase
    {
        public string Id { get; set; }

        [Inject]
        public IAdministrationService administrationService { get; set; }

        public RoleDto Role { get; set; } = new RoleDto(); 

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string ErrorMessage { get; set; }

        public List<RoleDto> RoleDtoList { get; set; } = new List<RoleDto>();

        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

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
