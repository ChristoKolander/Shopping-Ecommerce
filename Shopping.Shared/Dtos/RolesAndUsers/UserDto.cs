using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shopping.Shared.Dtos.RolesAndUsers
{
    public class UserDto
    {
        public UserDto()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
