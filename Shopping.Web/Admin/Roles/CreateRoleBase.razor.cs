using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace Shopping.Web.Admin.Roles
{
    public class CreateRoleBase: ComponentBase
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }


        public CreateRoleDto CreateRoleDto = new CreateRoleDto();

        [Inject]
        public IAdministrationService administrationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task CreateRole()
        {
            CreateRoleDto Success;

            Success = await administrationService.CreateRole(CreateRoleDto);

            if (Success.Name.Length > 0)
            {
                IsVisible = true;
                RecordName = CreateRoleDto.Name;
                Result = true;
                Edit = true;
                
            }
        }
    }
}
