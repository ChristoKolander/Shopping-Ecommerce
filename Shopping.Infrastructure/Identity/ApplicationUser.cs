using Microsoft.AspNetCore.Identity;
using System;

namespace Shopping.Infrastructure.Identity
{
   public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
 
       
    }
}
