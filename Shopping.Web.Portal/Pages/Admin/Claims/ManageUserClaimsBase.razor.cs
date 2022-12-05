using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Shared.Claims;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages.Admin.Claims
{
    public class ManageUserClaimsBase: ComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Selected { get; set; }
        public int Number { get; set; }

        public UserClaim UserClaim = new UserClaim();

        public EditUserDto EditUserDto = new EditUserDto();
        public UserClaimsDto UserClaimsDto { get; set; } = new UserClaimsDto();


        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

        [Inject]
        public IAdministrationService administrationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;


        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckAdminPolicy = await AuthService.AuthorizeAsync(authState.User, "AdminRolePolicy");

            if (CheckAdminPolicy.Succeeded)
            {
                EditUserDto = await administrationService.EditUser(Id);
                UserClaimsDto = await administrationService.ManageUserClaims(EditUserDto.Id);
            }
               
        }

        public async Task EditUserClaims()
        {

            UserClaimsDto Success;

            Success = await administrationService.ManageUserClaims(EditUserDto.Id, UserClaimsDto);


            if (Success.Claims.Count >= 0)
            {
                IsVisible = true;
                RecordName = EditUserDto.UserName;
                Result = true;
                Edit = true;

            }

            StateHasChanged();

        }


        public void Cancel_Click()
        {
            NavigationManager.NavigateTo($"/admin/edituser/{EditUserDto.Id}");

        }
    }
}
