using System.ComponentModel.DataAnnotations;


namespace Shopping.Shared.Dtos.RolesAndUsers
{
    public class RoleDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; } 
    }
}
