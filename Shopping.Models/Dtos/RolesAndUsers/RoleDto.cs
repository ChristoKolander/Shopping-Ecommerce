using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shopping.Models.Dtos.RolesAndUsers
{
    public class RoleDto
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; } 
    }
}
