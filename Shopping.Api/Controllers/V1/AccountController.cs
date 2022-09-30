using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shopping.Api.Entities;
using Shopping.Api.LoggerService;
using Shopping.Api.TokenHelpers;
using Shopping.Models.Dtos.Auths;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        #region Fields and CTOR

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly ILoggerManager logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 ITokenService tokenService,
                                 ILoggerManager logger,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.logger = logger;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        #endregion


        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var newUser = new ApplicationUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            // Maybe add encryption to password here? An extra layer of sec?
            var result = await userManager.CreateAsync(newUser, userForRegistration.Password);

            if (!result.Succeeded)
            {
                logger.LogError("Could not Register new Appplication User");
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await userManager.AddToRoleAsync(newUser, "Administrator");

            return StatusCode(201);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDto userForAuthentication)
        {
            var user = await userManager.FindByNameAsync(userForAuthentication.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = tokenService.GetSigningCredentials();
            var claims = await tokenService.GetClaims(user);
            var tokenOptions = tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });

        }

    }  
}
