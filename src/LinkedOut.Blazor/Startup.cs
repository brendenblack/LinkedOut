using LinkedOut.Application;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Blazor.Data;
using LinkedOut.Blazor.Services;
using LinkedOut.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();

            services.AddLinkedOut();
            services.AddInfrastructure(Configuration);

            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5001;
            //});
            //services.AddAuthorizationCore(opt =>
            //{
            //    //opt.
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies", opts =>
            {
                opts.Cookie.SameSite = SameSiteMode.None;
                opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = Configuration.GetSection("Auth").GetValue<string>("Authority");
                options.ClientId = Configuration.GetSection("Auth").GetValue<string>("ClientId");
                options.ClientSecret = Configuration.GetSection("Auth").GetValue<string>("ClientSecret");
                options.ResponseType = Configuration.GetSection("Auth").GetValue<string>("ResponseType", "code");
                options.SaveTokens = Configuration.GetSection("Auth").GetValue<bool>("SaveTokens", false);
                options.GetClaimsFromUserInfoEndpoint = Configuration.GetSection("Auth").GetValue<bool>("GetClaimsFromUserInfoEndpoint", true);
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "email",
                };

                // sets to false is we're in development environment
                options.RequireHttpsMetadata = !_hostingEnvironment.IsDevelopment();



                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        Console.WriteLine($"OnAccessDenied to {context.AccessDeniedPath.Value}");
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        Console.WriteLine($"OnRemoteFailure: {context.Failure.Message}");
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        Debug.WriteLine(context.Failure.GetType().FullName);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, OidcCurrentUserService>();

            services.AddSingleton<BlazorServerAuthStateCache>();
            services.AddScoped<AuthenticationStateProvider, BlazorServerAuthState>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseForwardedHeaders();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

    }
}
