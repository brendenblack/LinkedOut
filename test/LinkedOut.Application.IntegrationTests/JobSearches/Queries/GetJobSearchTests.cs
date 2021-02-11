using LinkedOut.Application.JobSearches.Queries.GetJobSearch;
using LinkedOut.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests.JobSearches.Queries
{
    public class GetJobSearchTests : TestBase
    {
        public GetJobSearchTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public async Task ShouldReturnDto()
        {
            var userid = await RunAsDefaultUserAsync();
            var search = new JobSearch(userid)
            {
                Title = "This is the search",
                Description = "Here we go again"
            };
            var app1 = new JobApplication(search, "The Organization", "The Title");
            var app2 = new JobApplication(search, "The Other Guys", "The Junior Title");
            await AddAsync(app1, app2);
            var query = new GetJobSearchDetailsQuery
            {
                JobSearchId = search.Id
            };

            var result = await SendAsync(query);

            result.Title.ShouldBe(search.Title);
            result.Description.ShouldBe(search.Description);
            result.Applications.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ShouldReturnJobApplicationSummaries()
        {
            var userid = await RunAsDefaultUserAsync();
            var search = new JobSearch(userid)
            {
                Title = "This is the search",
                Description = "Here we go again"
            };
            var app1 = new JobApplication(search, "The Organization", "The Title");
            app1.Submit(DateTime.Now);
            
            await AddAsync(app1);
            var query = new GetJobSearchDetailsQuery
            {
                JobSearchId = search.Id
            };

            var result = await SendAsync(query);

            var applicationDto = result.Applications.FirstOrDefault(a => a.Id == app1.Id);
            applicationDto.ShouldNotBeNull();
            applicationDto.CurrentStatus.ShouldBe(ApplicationStatuses.SUBMITTED);
            applicationDto.AppliedOn.HasValue.ShouldBeTrue();
            applicationDto.AppliedOn.Value.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }
}
