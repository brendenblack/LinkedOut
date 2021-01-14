using LinkedOut.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class BlazorServerAuthState : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IDateTime _dateTime;
        private readonly BlazorServerAuthStateCache Cache;

        public BlazorServerAuthState(ILoggerFactory loggerFactory, IDateTime dateTime, BlazorServerAuthStateCache cache)
            : base(loggerFactory)
        {
            _dateTime = dateTime;
            Cache = cache;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10); // TODO read from config

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var sid = authenticationState.User.Claims
                .Where(c => c.Type.Equals("sid"))
                .Select(c => c.Value)
                .FirstOrDefault();

            if (sid != null && Cache.HasSubjectId(sid))
            {
                var data = Cache.Get(sid);
                if (_dateTime.Now >= data.Expiration)
                {
                    Cache.Remove(sid);
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
    }
}
