﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Models.Dtos.Auths;
using Shopping.Web.Pages.Auth;
using Shopping.Web.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;




namespace Shopping.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        #region Fields and CTOR

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthenticationService(HttpClient httpClient, 
                                     AuthenticationStateProvider authStateProvider, 
                                     ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;
        }

        #endregion

        public async Task<RegistrationResponseDto> RegisterUser(UserRegistrationDto userForRegistration)
        {
            var jsonRequest = JsonSerializer.Serialize(userForRegistration);
            var bodyContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage registrationResult = await httpClient.PostAsync("api/V1/account/registration", bodyContent);

            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (!registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, options);
                return result;
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true };

        }

        public async Task<AuthResponseDto> Login(UserAuthenticationDto userForAuthentication)
        {
            var jsonRequest = JsonSerializer.Serialize(userForAuthentication);
            var bodyContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage authResult = await httpClient.PostAsync("api/v1/account/login", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, options);

            if (!authResult.IsSuccessStatusCode)
                return result;

            await localStorage.SetItemAsync("authToken", result.Token);
            await localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(result.Token);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return new AuthResponseDto { IsAuthSuccessful = true };
        }
        
        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");
            await localStorage.RemoveItemAsync("refreshToken");

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await localStorage.GetItemAsync<string>("refreshToken");

            var tokenDto = System.Text.Json.JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");

            HttpResponseMessage refreshResult = await httpClient.PostAsync("api/v1/token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, options);

            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");

            await localStorage.SetItemAsync("authToken", result.Token);
            await localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return result.Token;
        }
      
    }
}

            

        
