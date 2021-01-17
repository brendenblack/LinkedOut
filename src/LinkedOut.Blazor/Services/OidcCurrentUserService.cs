using LinkedOut.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Services
{
    /// <summary>
    /// Retrieves details about the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="AuthenticationStateProvider"/>, because Blazor does not populate <see cref="HttpContext"/> when the application is deployed.
    /// See <a href="https://mcguirev10.com/2019/12/16/blazor-login-expiration-with-openid-connect.html">this blog post</a> for more details.
    /// </remarks>
    public class OidcCurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public OidcCurrentUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        // hack to allow seeding the database to work
        public string UserIdOverride { get; set; }

        public string UserId
        {
            get
            {
                try
                {
                    return _authenticationStateProvider.GetAuthenticationStateAsync().Result.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public bool IsAuthenticated => _authenticationStateProvider.GetAuthenticationStateAsync().Result.User?.Identity?.IsAuthenticated ?? false;

        public string FirstName => _authenticationStateProvider.GetAuthenticationStateAsync().Result.User?.FindFirstValue(ClaimTypes.GivenName) ?? "";

        public string LastName => _authenticationStateProvider.GetAuthenticationStateAsync().Result.User?.FindFirstValue(ClaimTypes.Surname) ?? "";

        public string Email => _authenticationStateProvider.GetAuthenticationStateAsync().Result.User?.FindFirstValue(ClaimTypes.Email) ?? "";

    }
}
