using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping.Api.Entities;
using Shopping.Api.LoggerService;
using Shopping.Api.TokenHelpers;
using Shopping.Models.Dtos.Auths;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Shopping.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        #region Fields and CTOR

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILoggerManager logger;

        public TokenController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILoggerManager logger)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        #endregion

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                logger.LogError("RefresTokenDto is null");
                return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });
               
            }

            var principal = tokenService.GetPrincipalFromExpiredToken(tokenDto.Token);
            var username = principal.Identity.Name;

            var user = await userManager.FindByEmailAsync(username);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Invalid client request" });

            var signingCredentials = tokenService.GetSigningCredentials();
            var claims = await tokenService.GetClaims(user);
            var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            user.RefreshToken = tokenService.GenerateRefreshToken();
           
            await userManager.UpdateAsync(user);
            
            return Ok(new AuthResponseDto { Token = token, RefreshToken = user.RefreshToken, IsAuthSuccessful = true });
        }

    }
}
