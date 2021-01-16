using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Pages
{
    public class _HostAuthModel : PageModel
    {
        private readonly ILogger<_HostAuthModel> _logger;

        public _HostAuthModel(ILogger<_HostAuthModel> logger, BlazorServerAuthStateCache cache)
        {
            _logger = logger;
            Cache = cache;
        }
        public BlazorServerAuthStateCache Cache { get; }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Get request from authenticated user");

                var sid = User.Claims
                    //.Where(c => c.Type.Equals("sid"))
                    .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                    .Select(c => c.Value)
                    .FirstOrDefault();

                if (sid != null)
                {
                    if (!Cache.HasSubjectId(sid))
                    {
                        var authResult = await HttpContext.AuthenticateAsync("oidc");
                        DateTimeOffset expiration = authResult.Properties.ExpiresUtc.Value;
                        string accessToken = await HttpContext.GetTokenAsync("access_token");
                        string refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                        Cache.Add(sid, expiration, accessToken, refreshToken);
                        _logger.LogTrace("Cached login token for Subject ID {SubjectID}, expiring on {Expiration}", sid, expiration);
                    }
                    else
                    {
                        _logger.LogTrace("Login token for Subject ID {SubjectID} is already cached", sid);
                    }
                }
                else
                {
                    _logger.LogWarning("An authenticated user is accessing a page without a Subject ID");
                }
            }
            else
            {
                _logger.LogDebug("Got request fron unauthenticated user");
            }

            return Page();
        }

        public IActionResult OnGetLogin()
        {
            _logger.LogDebug("Getting login");
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(15), // TODO: provide a sensible value, or from config
                RedirectUri = Url.Content("~/")
            };
            return Challenge(authProps, "oidc");
        }

        public async Task OnGetLogout()
        {
            _logger.LogDebug("Getting logout");
            var authProps = new AuthenticationProperties
            {
                RedirectUri = Url.Content("~/")
            };
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc", authProps);
        }
    }
}
