using Microsoft.IdentityModel.Tokens;
using Shopping.Api.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shopping.Api.TokenHelpers
{
    public interface ITokenService
    {
        string GenerateRefreshToken();

        SigningCredentials GetSigningCredentials();

        Task<List<Claim>> GetClaims(ApplicationUser user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);     

    }
}
