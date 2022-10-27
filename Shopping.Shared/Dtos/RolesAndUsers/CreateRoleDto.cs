using System.ComponentModel.DataAnnotations;


namespace Shopping.Shared.Dtos.RolesAndUsers
{
   public class CreateRoleDto
    {
       
        [Required]
        public string Name { get; set; }

    }
}
