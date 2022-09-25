using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Admin.Roles
{
    public class EditRoleBase : ComponentBase
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string Id { get; set; }

        public int number { get; set; }


        protected Shopping.Web.Components.ConfirmBase DeleteConfirmation { get; set; }


        public EditRoleDto EditRoleDto = new EditRoleDto();

        public UserRoleDto UserRoleDto = new UserRoleDto();

        public RoleDto RoleDto = new RoleDto();

        public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();

        [Inject]
        public IAdministrationService administrationService { get; set; }


        protected async override Task OnInitializedAsync()
        {

            EditRoleDto = await administrationService.GetRole(Id);
            UserRoleDtos = await administrationService.GetUsersInRole(EditRoleDto.Id);

            number = UserRoleDtos.Count;
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


