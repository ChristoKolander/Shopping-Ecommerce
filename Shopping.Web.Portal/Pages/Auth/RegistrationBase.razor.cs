using Microsoft.AspNetCore.Components;
using Shopping.Shared.Dtos.Auths;
using Shopping.Web.Portal.Services.Interfaces;


namespace Shopping.Web.Portal.Pages.Auth
{
    public class RegistrationBase : ComponentBase
    {
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public UserRegistrationDto userRegistrationDto = new UserRegistrationDto();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
  

        public async Task Register()
        {
            ShowRegistrationErrors = false;

            var newUser = await AuthenticationService.RegisterUser(userRegistrationDto);

            if (!newUser.IsSuccessfulRegistration)
            {
                Errors = newUser.Errors;
                ShowRegistrationErrors = true;
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }

        }
    }
}

   


 

    
    
