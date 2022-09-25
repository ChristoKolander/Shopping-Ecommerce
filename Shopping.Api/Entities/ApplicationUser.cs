using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
