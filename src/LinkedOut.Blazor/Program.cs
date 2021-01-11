using LinkedOut.Infrastructure.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
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
                var config = services.GetRequiredService<IConfiguration>();
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(config)
                    .CreateLogger();

                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (!context.Database.IsInMemory())
                    {
                        var scopeDictionary = new Dictionary<string, object>
                        {
                            ["OperationType"] = "dbseed"
                        };

                        using (logger.BeginScope(scopeDictionary))
                        {
                            logger.LogInformation("Attempting to migrate the database...");
                            context.Database.Migrate();
                            logger.LogInformation("Migration complete");
                        }
                    }

                    var env = services.GetService<IHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        logger.LogInformation("Starting up in Development environment");
                        var testUserId = services.GetRequiredService<IConfiguration>().GetValue("TestUserId", Guid.NewGuid().ToString());
                        await ApplicationDbContextSeed.SeedTestData(context, testUserId);
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database");
                }               
            }

            try
            {
                Log.Logger.Information("Starting web host");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
