using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.RolesAndUsers;

namespace Shopping.Web.Portal.Services.Interfaces
{
   public interface IAdministrationService
    {
        Task<EditRoleDto> GetRole(string Id);
        Task<List<RoleDto>> GetRoles();
        Task<EditRoleDto> EditRole(EditRoleDto editRoleDto);
        Task<CreateRoleDto> CreateRole(CreateRoleDto createRoleDto);
        Task<RoleDto> DeleteRole(string Id);

        Task<List<UserRoleDto>> GetUsersInRole(string Id);
        Task<UserDto> GetRolesForUser(UserDto userDto);

        Task<List<UserRolesDto>> ManageUserRoles(string Id);
        Task<List<UserRolesDto>> ManageUserRoles(string Id, List<UserRolesDto>userRolesDtos);

        Task<List<UserRoleDto>> ManageRoleMemberShip(string Id);
        Task<List<UserRoleDto>> ManageRoleMemberShip(string Id, List<UserRoleDto> userRoleDtos);

        Task<UserClaimsDto> ManageUserClaims(string Id);
        Task<UserClaimsDto> ManageUserClaims(string Id, UserClaimsDto userClaimsDto);

        Task<List<UserDto>> GetUsers();       
        Task<EditUserDto> EditUser(string Id);
        Task<EditUserDto> EditUser(EditUserDto editUserDto);
        Task<UserDto> DeleteUser(string id);
    }
}

