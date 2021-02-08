using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Infrastructure.Persistence;
using LinkedOut.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinkedOut.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // We support multiple database types of in memory, for local dev, PostgreSQL because it's free and SqlServer 
            // for simple LocalDb integration testing and aspirations of some day making it
            // This is achieved in a couple of steps, 
            // 1. If an environment variable of DATABASE_URL is set (Heroku convention) then we'll use that to set
            //    up PostgreSQL specific to Heroku
            // 2. Otherwise, setting the "DatabaseType" config value to one of inmemory, postgresql or sqlserver will
            //    tell the app how to wire things up
            //
            // There are a couple of big gotchas with this system.
            // First, we have to support multiple DbContexts. It was technically possible to do this with one context, but 
            // that requires splitting migrations in to different assemblies. See this article for more:
            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli#using-multiple-context-types
            // Second, we have to generate multiple sets of migrations that specify the context and output directory. There is a
            // script that makes this process easier found in the project root called create_migrations.bat that takes an argument 
            // of the migration name.
            var databaseType = configuration.GetValue("DatabaseType", "InMemory");
            if (TryGetHerokuConnectionString(out string herokuConnectionString))
            {
                Console.WriteLine("Configuring database for Heroku-specific PostgreSQL");
                // TODO: i don't love putting this in code
                services.AddDbContext<PostgreSqlApplicationDbContext>(options =>
                    options.UseNpgsql(herokuConnectionString)
                        .UseSnakeCaseNamingConvention());

                services.AddScoped<IApplicationDbContext>(provider => provider.GetService<PostgreSqlApplicationDbContext>());
            }
            else if (databaseType.Equals("PostgreSQL", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Configuring database for PostgreSQL");
                services.AddDbContext<PostgreSqlApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention());

                services.AddScoped<IApplicationDbContext>(provider => provider.GetService<PostgreSqlApplicationDbContext>());

                //services.AddDbContext<ApplicationDbContext>(options =>
                //    options.UseNpgsql(
                //            configuration.GetConnectionString("DefaultConnection"),
                //            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                //    .UseSnakeCaseNamingConvention());
            }
            else if (databaseType.Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Configuring database for SqlServer");
                services.AddDbContext<SqlServerApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(SqlServerApplicationDbContext).Assembly.FullName)), ServiceLifetime.Transient);

                services.AddScoped<IApplicationDbContext>(provider => provider.GetService<SqlServerApplicationDbContext>());

                //services.AddDbContextCheck<SqlServerApplicationDbContext>();
            }
            else
            {
                Console.WriteLine("Configuring in-memory database");
                services.AddDbContext<SqlServerApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("LinkedOutDb"));
                services.AddScoped<IApplicationDbContext>(provider => provider.GetService<SqlServerApplicationDbContext>());
            }

            services.AddTransient<INewUserService, FuturamaNewUserService>();
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }

        public static bool TryGetHerokuConnectionString(out string connectionString)
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (string.IsNullOrWhiteSpace(environmentConnectionString))
            {
                connectionString = "";
                // no environment variable set, we're not running on Heroku
                return false;
            }

            var databaseUri = new Uri(environmentConnectionString);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
            connectionString = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
            return true;
        }
    }
}
