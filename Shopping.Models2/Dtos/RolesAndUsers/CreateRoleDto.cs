using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shopping.Models.Dtos.RolesAndUsers
{
   public class CreateRoleDto
    {
       
        [Required]
        public string Name { get; set; }

    }
}
