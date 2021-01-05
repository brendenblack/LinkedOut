using LinkedOut.Infrastructure.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (!context.Database.IsInMemory())
                    {
                        logger.LogInformation("Attempting to migrate the database...");
                        context.Database.Migrate();
                    }

                    var env = services.GetService<IHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        logger.LogDebug("Starting up in Development environment");
                        var testUserId = services.GetRequiredService<IConfiguration>().GetValue("TestUserId", Guid.NewGuid().ToString());
                        await ApplicationDbContextSeed.SeedTestData(context, testUserId);
                    }

                    //var server = services.GetService<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

                   
                    logger.LogInformation("We're starting up!");
                    
                }
                catch (Exception ex)
                {
                    //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database");
                }

                
            }

            

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Heroku requirement
                    // https://medium.com/swlh/deploy-your-net-core-3-1-application-to-heroku-with-docker-eb8c96948d32
                    var port = Environment.GetEnvironmentVariable("PORT");
                    if (!string.IsNullOrWhiteSpace(port))
                    {
                        webBuilder.UseUrls("http://*:" + Environment.GetEnvironmentVariable("PORT"));
                    }
                });
    }
}
