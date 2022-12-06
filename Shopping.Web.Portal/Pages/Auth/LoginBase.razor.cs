using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.Auths;
using Shopping.Web.Portal.Services.Interfaces;
using Shopping.Web.Portal.Shared;

namespace Shopping.Web.Portal.Pages.Auth
{
    public class LoginBase : ComponentBase
    {

        public string Result { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        public UserAuthenticationDto userForAuthentication = new UserAuthenticationDto();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        
        public AuthSolution authSolution { get; set; } = new AuthSolution();


        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await AuthenticationService.Login(userForAuthentication);
            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
        
            }
            else
            {
               
                NavigationManager.NavigateTo("/", forceLoad: true);

            }

        }

    }
}
