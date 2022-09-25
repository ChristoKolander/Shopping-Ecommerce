using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos.Auths;
using Shopping.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Web.Pages.Auth
{
    public class RegistrationBase : ComponentBase
    {
        public UserRegistrationDto userRegistrationDto = new UserRegistrationDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

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

   


 

    
    
