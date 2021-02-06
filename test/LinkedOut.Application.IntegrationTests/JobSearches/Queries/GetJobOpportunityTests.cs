using LinkedOut.Application.JobSearches.Queries.GetJobOpportunity;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.ValueObjects;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests.JobSearches.Queries
{
    public class GetJobOpportunityTests : TestBase
    {
        public GetJobOpportunityTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public async Task ShouldReturnBasicDetails()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId)
            {
                Title = "Job Pilot 3000",
            };
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            application.AddNote("Woohoo, I'm a delivery boy!");
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.ParentSearchId.ShouldBe(search.Id);
            result.ParentSearchTitle.ShouldBe("Job Pilot 3000");
            result.OrganizationName.ShouldBe("Planet Express");
            result.JobTitle.ShouldBe("Delivery Boy");
        }

        #region Applied status
        [Fact]
        public async Task ShouldIndicateNotApplied_WhenNotSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.DidApply.ShouldBeFalse();
        }

        [Fact]
        public async Task ShouldIndicateApplied_WhenSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            application.Submit(DateTime.Now);
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.DidApply.ShouldBeTrue();
        }
        #endregion

        
        #region Location tests
        [Fact]
        public async Task ShouldReturnPartsUnknown_WhenNoLocation()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.Location.ToString().ShouldBe("Parts Unknown");
        }

        [Fact]
        public async Task ShouldReturnLocationString_WhenLocationSet()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            application.Location = Location.Toronto;
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.Location.ToString().ShouldBe(Location.Toronto.ToString());
        }

        [Fact]
        public async Task ShouldReturnCityNameWithNoTrailingComma_WhenLocationHasNoProvince()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            application.Location = new Location("New New York", "");
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            result.Location.ToString().ShouldBe("New New York");
        }
        #endregion
    }
}
