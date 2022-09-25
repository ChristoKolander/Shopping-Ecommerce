using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Models.Dtos.Auths
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
