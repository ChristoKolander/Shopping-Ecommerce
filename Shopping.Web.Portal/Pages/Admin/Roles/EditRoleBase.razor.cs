using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;

namespace Shopping.Web.Portal.Pages.Admin.Roles
{
    public class EditRoleBase : ComponentBase
    {

        [Parameter]
        public string Id { get; set; }

        public int Number { get; set; }

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        public RoleDto RoleDto = new RoleDto();
        public EditRoleDto EditRoleDto = new EditRoleDto();
        public UserRoleDto UserRoleDto = new UserRoleDto();
        public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();

        protected Shopping.Web.Portal.Components.ConfirmBase DeleteConfirmation { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

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
                EditRoleDto = await administrationService.GetRole(Id);
                UserRoleDtos = await administrationService.GetUsersInRole(EditRoleDto.Id);
            }
         
             Number = UserRoleDtos.Count;
        }


        public async Task EditRole()
        {

            EditRoleDto Success;

            Success = await administrationService.EditRole(EditRoleDto);


            if (Success.Name.Length > 0)
            {
                IsVisible = true;
                RecordName = EditRoleDto.Name;
                Result = true;
                Edit = true;


            }

        }


        public async void GoBack()
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
        
                await administrationService.DeleteRole(EditRoleDto.Id);


                if (EditRoleDto.Name.Length>1)
                {
                    IsVisible = true;
                    RecordName = EditRoleDto.Name;
                    Result = true;
                    Delete = true;


                }

            }

        }
    }
}


