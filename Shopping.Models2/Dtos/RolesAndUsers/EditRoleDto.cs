using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shopping.Models.Dtos.RolesAndUsers
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
