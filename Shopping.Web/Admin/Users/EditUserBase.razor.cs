using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Web.Admin.Users
{
    public class EditUserBase: ComponentBase
    {

        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        [Parameter]
        public string Id { get; set; }

        public EditUserDto EditUserDto = new EditUserDto();

        public UserRoleDto UserRoleDto = new UserRoleDto();

        public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();

        protected Shopping.Web.Components.ConfirmBase DeleteConfirmation { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAdministrationService administrationService { get; set; }

        protected async override Task OnInitializedAsync()
        {

            EditUserDto = await administrationService.EditUser(Id);
          
        }

        public async Task EditUser()
        {

            EditUserDto Success;

            Success = await administrationService.EditUser(EditUserDto);


            if (Success.UserName.Length > 0)
            {
                IsVisible = true;
                RecordName = EditUserDto.UserName;
                Result = true;
                Edit = true;


            }

        }

        public void Cancel_Click()
        {
            NavigationManager.NavigateTo("admin/users");
           

        }

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {

                await administrationService.DeleteUser(EditUserDto.Id);

                IsVisible = true;
                RecordName = EditUserDto.UserName;
                Result = true;
                Delete = true;
            }

        }
    }
}
