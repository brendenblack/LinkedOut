using LinkedOut.Application.JobSearches.Queries.GetJobSearchesForUser;
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
    public class GetJobSearchForUserTests : TestBase
    {
        public GetJobSearchForUserTests(ITestOutputHelper output)
            : base(output)
        { }

        [Fact]
        public async Task ShouldReturnAllSearchesOwnedByUser()
        {
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new JobSearch(userId));
            await AddAsync(new JobSearch(userId));
            // this search does not belong to the user, and should not be returned
            await AddAsync(new JobSearch("not-my-user") { Title = "invalid result" } );

            var query = new GetJobSearchesForUserQuery
            {
                UserId = userId
            };

            var result = await SendAsync(query);

            result.JobSearches.Count.ShouldBe(2);
            result.JobSearches.Any(s => s.Title == "invalid result").ShouldBeFalse();
        }

        [Fact]
        public async Task ShouldReturnApplicationCount()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var app1 = new JobApplication(search, "The Organization", "Job 1");
            var app2 = new JobApplication(search, "The Organization", "Job 2");
            await AddAsync(app1, app2);

            var query = new GetJobSearchesForUserQuery
            {
                UserId = userId
            };

            var result = await SendAsync(query);

            var searchDto = result.JobSearches.Where(s => s.Id == search.Id).FirstOrDefault();
            searchDto.ShouldNotBeNull();
            searchDto.ApplicationsCount.ShouldBe(2);
        }

        [Fact]
        public async Task ShouldReturnBasicDetails()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId)
            {
                Title = "A search for the ages",
            };
            
            var app1 = new JobApplication(search, "The Organization", "Job 1");
            var app2 = new JobApplication(search, "The Organization", "Job 2");
            await AddAsync(app1, app2);

            var query = new GetJobSearchesForUserQuery
            {
                UserId = userId
            };

            var result = await SendAsync(query);

            var searchDto = result.JobSearches.Where(s => s.Id == search.Id).FirstOrDefault();
            searchDto.ShouldNotBeNull();
            searchDto.Title.ShouldBe("A search for the ages");
            searchDto.Created.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }
}
