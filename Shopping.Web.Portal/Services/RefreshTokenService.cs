using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Web.Portal.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Shopping.Web.Portal.Services
{
    public class RefreshTokenService
    {

        #region Fields and CTOR

        private readonly AuthenticationStateProvider authProvider;
        private readonly IAuthenticationService authService;
       
        public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthenticationService authService)
        {
            this.authProvider = authProvider;
            this.authService = authService;
        }


        #endregion

        public async Task<string> TryRefreshToken()
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 2)
                return await authService.RefreshToken();
            return string.Empty;

        }
    }
}
