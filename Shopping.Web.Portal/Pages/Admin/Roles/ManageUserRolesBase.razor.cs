using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages.Admin.Roles
{
    public class ManageUserRolesBase: ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Selected { get; set; }
        public int Number { get; set; }


        public EditUserDto EditUserDto = new EditUserDto();
        public UserRolesDto UserRolesDto = new UserRolesDto();
        public List<UserRolesDto> UserRolesDtos { get; set; } = new List<UserRolesDto>();

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
                EditUserDto = await administrationService.EditUser(Id);
                UserRolesDtos = await administrationService.ManageUserRoles(EditUserDto.Id);
            }
        }

        public async Task EditRoleMemberShip()
        {

            List<UserRolesDto> Success;

            Success = await administrationService.ManageUserRoles(EditUserDto.Id, UserRolesDtos);


            if (Success.Count >= 0)
            {
                IsVisible = true;
                RecordName = EditUserDto.UserName;
                Result = true;
                Edit = true;

            }

            StateHasChanged();

        }


    }
}
