using LinkedOut.Application;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Blazor.Data;
using LinkedOut.Blazor.Services;
using LinkedOut.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddLinkedOut();
            services.AddInfrastructure(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Configuration.GetSection("Auth").GetValue<string>("Authority");
                options.ClientId = Configuration.GetSection("Auth").GetValue<string>("ClientId");
                options.ClientSecret = Configuration.GetSection("Auth").GetValue<string>("ClientSecret");
                options.ResponseType = Configuration.GetSection("Auth").GetValue<string>("ResponseType", "code");
                options.SaveTokens = Configuration.GetSection("Auth").GetValue<bool>("SaveTokens", true);
                options.GetClaimsFromUserInfoEndpoint = Configuration.GetSection("Auth").GetValue<bool>("GetClaimsFromUserInfoEndpoint", true);
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                
                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        Debug.WriteLine(context.Failure.GetType().FullName);
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
