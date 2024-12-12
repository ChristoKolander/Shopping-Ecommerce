using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using Shopping.Shared.Dtos.Auths;
using Shopping.Api.Attributes;
using Shopping.Core.Interfaces;
using Shopping.Shared;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        #region Fields and CTOR

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ILoggerManager logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 ITokenService tokenService,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration,
                                 ILoggerManager logger)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.logger = logger;
        }

        #endregion


        [HttpPost("Registration")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userForRegistration)
        {
           
            var newUser = new ApplicationUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            // Maybe add encryption to password here? An extra layer of sec?
            var result = await userManager.CreateAsync(newUser, userForRegistration.Password);

            if (!result.Succeeded)
            {
                logger.LogError("Could not Register new Appplication User");
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await userManager.AddToRoleAsync(newUser, "Administrators");

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
