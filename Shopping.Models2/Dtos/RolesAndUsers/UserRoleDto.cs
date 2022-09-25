using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Models.Dtos.RolesAndUsers
{
    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool Selected { get; set; }

        //public string RoleId { get; set; }
        //public string RoleName { get; set; }


    }
}
