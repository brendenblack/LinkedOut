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
            if (databaseType.Equals("PostgreSQL", StringComparison.InvariantCultureIgnoreCase))
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
    }
}
