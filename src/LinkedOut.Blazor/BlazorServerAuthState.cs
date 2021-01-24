using LinkedOut.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class BlazorServerAuthState : RevalidatingServerAuthenticationStateProvider
    {
        private readonly ILogger<BlazorServerAuthState> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDateTime _dateTime;
        private readonly BlazorServerAuthStateCache _cache;

        public BlazorServerAuthState(ILogger<BlazorServerAuthState> logger,
                                     IConfiguration configuration,
                                     ILoggerFactory loggerFactory,
                                     IDateTime dateTime,
                                     BlazorServerAuthStateCache cache)
            : base(loggerFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _dateTime = dateTime;
            _cache = cache;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(_configuration.GetSection("Auth").GetValue("LoginRevalidationInterval", 30));

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var sid = authenticationState.User.Claims
                //.Where(c => c.Type.Equals("sid"))
                .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Select(c => c.Value)
                .FirstOrDefault();

            if (sid != null)
            {
                var name = authenticationState.User.Claims
                        .Where(c => c.Type.Equals(ClaimTypes.Name))
                        .Select(c => c.Value)
                        .FirstOrDefault() ?? "";

                var scopeDictionary = new Dictionary<string, object>
                {
                    ["SubjectID"] = sid,
                    ["Name"] = name
                };
                using (_logger.BeginScope(scopeDictionary))
                {
                    if (_cache.HasSubjectId(sid))
                    {

                        _logger.LogTrace("Validating login token...");

                        var data = _cache.Get(sid);

                        _logger.LogTrace("Login token expires at {TokenExpiry}", data.Expiration.ToString("o"));

                        if (_dateTime.Now >= data.Expiration)
                        {
                            _logger.LogTrace("Login token is expired");
                            _cache.Remove(sid);
                            return Task.FromResult(false);
                        }
                        else
                        {
                            _logger.LogTrace("Login token is still valid");
                        }
                    }
                    else
                    {
                        // Populating the cache is the responsible for _HostAuthModel#OnGet
                        _logger.LogWarning("Attempting to validate login token, but was not found in the cache", sid);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Attempting to validate a login token for a user with a null Subject ID");
            }

            return Task.FromResult(true);
        }
    }
}
