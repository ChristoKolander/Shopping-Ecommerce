using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Web.Pages.Admin.Roles
{
    public class ManageRoleMemberShipBase : ComponentBase
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Selected { get; set; }
        public int Number { get; set; }

        [Parameter]
        public string Id { get; set; }

        public EditRoleDto EditRoleDto = new EditRoleDto();

        public UserRoleDto UserRoleDto = new UserRoleDto();

        public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();
    
        [Inject]
        public IAdministrationService administrationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


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
                EditRoleDto = await administrationService.GetRole(Id);
                UserRoleDtos = await administrationService.ManageRoleMemberShip(EditRoleDto.Id);
            }                
        }

        public async Task EditMemberShip()
        {

            List<UserRoleDto> Success;

            Success = await administrationService.ManageRoleMemberShip(EditRoleDto.Id, UserRoleDtos);


            if (Success.Count >=0)
            {
                IsVisible = true;
                RecordName = EditRoleDto.Name;
                Result = true;
                Edit = true;
            }
         
           
        }
        
    }
           
}




