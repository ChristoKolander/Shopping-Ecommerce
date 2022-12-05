using Shopping.Shared;
using Shopping.Shared.Dtos.Auths;

namespace Shopping.Web.Portal.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserAuthenticationDto userForAuthentication);
        Task Logout();
        Task<string> RefreshToken();

        //Not used at the moment.
        Task<CurrentUser> CurrentUserInfo();

    }
}

