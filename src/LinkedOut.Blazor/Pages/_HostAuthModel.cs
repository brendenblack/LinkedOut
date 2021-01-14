using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Pages
{
    public class _HostAuthModel : PageModel
    {
        public _HostAuthModel(BlazorServerAuthStateCache cache)
        {
            Cache = cache;
        }
        public BlazorServerAuthStateCache Cache { get; }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var sid = User.Claims
                    .Where(c => c.Type.Equals("sid"))
                    .Select(c => c.Value)
                    .FirstOrDefault();

                if (sid != null && !Cache.HasSubjectId(sid))
                {
                    var authResult = await HttpContext.AuthenticateAsync("oidc");
                    DateTimeOffset expiration = authResult.Properties.ExpiresUtc.Value;
                    string accessToken = await HttpContext.GetTokenAsync("access_token");
                    string refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                    Cache.Add(sid, expiration, accessToken, refreshToken);
                }
            }

            return Page();
        }

        public IActionResult OnGetLogin()
        {
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(15),
                RedirectUri = Url.Content("~/")
            };
            return Challenge(authProps, "oidc");
        }

        public async Task OnGetLogout()
        {
            var authProps = new AuthenticationProperties
            {
                RedirectUri = Url.Content("~/")
            };
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc", authProps);
        }
    }
}
