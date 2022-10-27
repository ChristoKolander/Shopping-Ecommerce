using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.Auths;
using Shopping.Web.Portal.Services.Interfaces;

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
               
                NavigationManager.NavigateTo("/");
            }

        }

    }
}
