using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Blazor.Services;
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
                   
                    // Attempt to migrate the database by first trying to get the SQL Server context. If it isn't found,
                    // we use GetRequiredService to fetch the PostgreSQL context, throwing an exception if it isn't available
                    SqlServerApplicationDbContext sqlServerContext = services.GetService<SqlServerApplicationDbContext>();
                    IApplicationDbContext context; 
                    if (sqlServerContext != null)
                    {
                        context = sqlServerContext;
                        ApplyMigration(logger, sqlServerContext);
                    }
                    else
                    {
                        var postgresqlContext = services.GetRequiredService<PostgreSqlApplicationDbContext>();
                        ApplyMigration(logger, postgresqlContext);
                        context = postgresqlContext;
                    }

                    var env = services.GetService<IHostEnvironment>();
                    if (env.IsDevelopment() || env.IsStaging())
                    {
                        // we only want to seed the database in staging or lower environments
                        logger.LogInformation("Performing database seed for {EnvironmentName}", env.EnvironmentName);
                        var testUserId = config.GetValue<string>("TestUserId", "");
                        if (string.IsNullOrWhiteSpace(testUserId))
                        {
                            testUserId = Guid.NewGuid().ToString();
                            logger.LogWarning("No test user id was supplied through a TestUserId configuration value. Using GUID {GUID} instead.", testUserId);
                        }

                        // this is a hack to ensure that our ICurrentUserService returns our seed value, instead of
                        // trying to call GetAuthenticationStateAsync before an AuthenticationState has been set

                        //OidcCurrentUserService oidcCurrentUserService = (OidcCurrentUserService)services.GetRequiredService<ICurrentUserService>();
                        //oidcCurrentUserService.UserIdOverride = testUserId;
                        await ApplicationDbContextSeed.SeedTestData(context, testUserId);
                        //oidcCurrentUserService.UserIdOverride = null;
                    }
                    else
                    {
                        logger.LogInformation("Skipping database seed for {EnvironmentName}", env.EnvironmentName);
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

        public static void ApplyMigration(ILogger<Program> logger, DbContext context)
        {
            var scopeDictionary = new Dictionary<string, object>
            {
                ["Method"] = "ApplyMigration",
                ["DatabaseType"] = context.Database.GetType().FullName
            };

            using (logger.BeginScope(scopeDictionary))
            {
                if (!context.Database.IsInMemory())
                {
                    logger.LogInformation("Attempting to migrate the database...");
                    context.Database.Migrate();
                    logger.LogInformation("Migration complete");   
                }
                else
                {
                    logger.LogInformation("Database is in memory, no migrations needed");
                }
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

    public class SeedCurrentUserService : ICurrentUserService
    {
        public SeedCurrentUserService(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }

        public bool IsAuthenticated => throw new NotImplementedException();

        public string FirstName => throw new NotImplementedException();

        public string Email => throw new NotImplementedException();
    }
}
