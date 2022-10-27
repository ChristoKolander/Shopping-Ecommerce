using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shopping.Shared.Dtos.RolesAndUsers
{
    public class EditRoleDto
    {

        public EditRoleDto()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } 

        public List<string> Users { get; set; }


    }
}
