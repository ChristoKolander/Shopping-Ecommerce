using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;

namespace Shopping.Web.Portal.Pages.Admin.Users
{
    public class UsersBase: ComponentBase
    {
        [Parameter]
        public UserDto User { get; set; } = new UserDto();

        [Parameter]
        public string Id { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public List<UserDto> UserDtoList { get; set; } = new List<UserDto>();

        public string ErrorMessage { get; set; }

        public Shopping.Web.Portal.Components.ConfirmBase DeleteConfirmation { get; set; }
 

        [Inject]
        public IAdministrationService administrationService { get; set; }
  
        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected async override Task OnInitializedAsync()
        {
            await GetUsers();

        }

        private async Task GetUsers()
        {
            UserDtoList = await administrationService.GetUsers();
            
        }


    }
}
