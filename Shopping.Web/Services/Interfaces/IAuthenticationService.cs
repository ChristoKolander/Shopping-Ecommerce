using Shopping.Models;
using Shopping.Models.Dtos.Auths;
using System.Threading.Tasks;

namespace Shopping.Web.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserAuthenticationDto userForAuthentication);
        Task Logout();
        Task<string> RefreshToken();

    }
}

