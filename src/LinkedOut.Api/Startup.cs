using LinkedOut.Api.Services;
using LinkedOut.Application;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });

            services.AddLinkedOut();
            services.AddInfrastructure(Configuration);

            services.AddControllers();



                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = Configuration.GetSection("Auth").GetValue<string>("Authority");
                        options.Audience = Configuration.GetSection("Auth").GetValue<string>("ClientId");
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                        };
                        options.RequireHttpsMetadata = HostingEnvironment.IsProduction();
                    });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("ApiScope", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "linkedoutapi");
                    });
                });
           

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    o.RequireHttpsMetadata = HostingEnvironment.IsProduction();
            //    o.Authority = Configuration.GetSection("Auth").GetValue<string>("Authority");
            //    o.Audience = Configuration.GetSection("Auth").GetValue<string>("ClientId");
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            c.NoResult();

            //            c.Response.StatusCode = 500;
            //            c.Response.ContentType = "text/plain";
            //            if (HostingEnvironment.IsDevelopment())
            //            {
            //                return c.Response.WriteAsync(c.Exception.ToString());
            //            }
            //            return c.Response.WriteAsync("An error occured processing your authentication.");
            //        }
            //    };
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", Configuration.GetSection("Auth").GetValue<string>("ClientId")));
            //    //options.AddPolicy("Administrator", policy => policy.RequireClaim("user_roles", "[Administrator]"));
            //});

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, HttpContextCurrentUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
