using LinkedOut.Api.IntegrationTests.TestHarness;
using LinkedOut.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Xunit.Abstractions;

namespace LinkedOut.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IServiceProvider _serviceProvider;
        public ITestOutputHelper Output { get; set; }

        public string SeededUserId { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {  
            builder.ConfigureTestServices(ConfigureServices);
            builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddProvider(new XUnitLoggerProvider(Output));
            });
        }

        public HttpClient GetAuthenticatedClient()
        {
            return GetAuthenticatedClient(SeededUserId, "bob@email.com");
        }

        public HttpClient GetAuthenticatedClient(string userId, string email = "")
        {
            var client = CreateClient();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, "Bob Bobberton"),
                new Claim(ClaimTypes.Email, (string.IsNullOrWhiteSpace(email)) ? "bob@email.com" : email),
            };

            var token = MockJwtService.GenerateJwtToken(claims);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            //if (setAsCurrentUser)
            //{
            //    var currentUserService = _serviceProvider.GetRequiredService<ICurrentUserService>();
            //    (currentUserService as TestCurrentUserService).UserId = userId;
            //}

            return client;
        }

        public IApplicationDbContext GetContext()
        {
            using (var scope = this.Services.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            }
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var config = new OpenIdConnectConfiguration()
                {
                    Issuer = MockJwtService.Issuer
                };
                config.SigningKeys.Add(MockJwtService.SecurityKey);

                options.Configuration = config;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void Dispose(bool disposing)
        {
            Log.CloseAndFlush();
            base.Dispose(disposing);
        }
    }
}

