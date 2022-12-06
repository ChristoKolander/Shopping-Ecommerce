using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Shopping.Shared;
using Shopping.Web.Portal.Features.RequestFeatures;
using Shopping.Web.Portal.Services.Interfaces;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Shopping.Web.Portal.Pages.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
     

        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }    

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           
            var token = await localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))

            return anonymous;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtExtract.ParseClaimsFromJwt(token), "jwtAuthType")));

        }


        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtExtract.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
            
        }


        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

      

    }
}
