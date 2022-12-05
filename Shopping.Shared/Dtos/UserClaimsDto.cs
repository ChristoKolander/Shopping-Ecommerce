using System.Collections.Generic;
using Shopping.Shared.Claims;

namespace Shopping.Shared.Dtos
{
    public class UserClaimsDto
    {
        public UserClaimsDto()
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
