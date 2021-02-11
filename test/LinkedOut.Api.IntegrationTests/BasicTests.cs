using LinkedOut.Api.IntegrationTests.TestHarness;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LinkedOut.Api.IntegrationTests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public BasicTests(ITestOutputHelper output, CustomWebApplicationFactory<Startup> factory)
        {
            factory.Output = output;
            _factory = factory;
        }

        [Fact]
        public async Task ShouldDo()
        {
            var userId = Guid.NewGuid().ToString();
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var search = new JobSearch(userId) { Title = "Job Search 3000" };
                var application = new JobApplication(search, "Planet Express", "Delivery Boy");
                context.JobApplications.Add(application);
                await context.SaveChangesAsync(new System.Threading.CancellationToken());
            }
            var client =  _factory.GetAuthenticatedClient(userId);

            var response = await client.GetAsync("/api/jobsearch");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ShouldUnauthorized()
        {
            var client = _factory.CreateDefaultClient();
            var response = await client.GetAsync("/api/jobsearch");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

    }
}
