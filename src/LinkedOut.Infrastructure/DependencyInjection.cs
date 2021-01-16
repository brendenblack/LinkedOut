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
            var databaseType = configuration.GetValue("DatabaseType", "InMemory");
            if (TryGetHerokuConnectionString(out string herokuConnectionString))
            {

                // TODO: i don't love putting this in code
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                        herokuConnectionString,
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            else if (databaseType.Equals("PostgreSQL", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                            configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            else if (databaseType.Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)), ServiceLifetime.Transient);
            }
            else
            { 
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("LinkedOutDb"));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

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
