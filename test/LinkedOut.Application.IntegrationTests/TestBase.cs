using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Blazor;
using LinkedOut.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests
{
    public class TestBase
    {
        protected IConfigurationRoot _configuration;
        protected IServiceScopeFactory _scopeFactory;
        protected Checkpoint _checkpoint;
        protected string _currentUserId;

        public TestBase(ITestOutputHelper output)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var host = Mock.Of<IHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "LinkedOut.Blazor");
            
            // use the host app's actual Startup file to configure our services
            var startup = new Startup(_configuration, host);

            var services = new ServiceCollection();
            services.AddSingleton(host); // inject a mock hosting environment
            startup.ConfigureServices(services);

            // Add XUnit logging
            services.AddLogging(cfg =>
            {
                cfg.AddProvider(new XUnitLoggerProvider(output));
                cfg.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
            });

            // Replace service registration for ICurrentUserService
            // Remove existing registration
            var currentUserServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICurrentUserService));
            services.Remove(currentUserServiceDescriptor);

            // Register testing version
            services.AddTransient(provider => Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
            ResetState().Wait();
        }

        protected void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();

            context.Database.Migrate();
        }

        protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        protected async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!", new string[] { });
        }

        public async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
        }

        public async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            //using var scope = _scopeFactory.CreateScope();

            //var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            //var user = new ApplicationUser { UserName = userName, Email = userName };

            //var result = await userManager.CreateAsync(user, password);

            //if (roles.Any())
            //{
            //    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            //    foreach (var role in roles)
            //    {
            //        await roleManager.CreateAsync(new IdentityRole(role));
            //    }

            //    await userManager.AddToRolesAsync(user, roles);
            //}

            //if (result.Succeeded)
            //{
            //_currentUserId = user.Id;
            _currentUserId = Guid.NewGuid().ToString();
            return _currentUserId;
            //}

            //var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            //throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }



        public async Task<TEntity> FindAsync<TEntity>(int id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();


            return await context.FindAsync<TEntity>(id);
        }

        public async Task<TEntity> FindAsync<TEntity>(int id, string[] referenceIncludes = null, string[] collectionIncludes = null)
            where TEntity : class
        {

            referenceIncludes ??= new string[] { };
            collectionIncludes ??= new string[] { };

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();
            var entity = await context.FindAsync<TEntity>(id);

            foreach (var include in referenceIncludes)
            {
                await context.Entry(entity).Reference(include).LoadAsync();
            }

            foreach (var include in collectionIncludes)
            {
                await context.Entry(entity).Collection(include).LoadAsync();
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Gross hack workaround, try not to use it. This is included because the generic <see cref="FindAsync{TEntity}(int)"/> methods
        /// aren't able to return related entities
        /// /// </remarks>
        public SqlServerApplicationDbContext GetContext()
        {
            using var scope = _scopeFactory.CreateScope();

            return scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();
        }

        public async Task<TEntity> FindAsync<TEntity>(string id)
        where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();

            return await context.FindAsync<TEntity>(id);
        }

        public async Task AddAsync<TEntity>(params TEntity[] entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();

            context.AddRange(entity);

            await context.SaveChangesAsync();
        }

        public async Task<TKey> AddAsync<TEntity, TKey>(TEntity entity, PropertyInfo key)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<SqlServerApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();

            return (TKey)key.GetValue(entity);
        }
    }
}
