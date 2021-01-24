using LinkedOut.Application.JobSearches.Queries.GetJobApplication;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.ValueObjects;
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
    public class GetJobApplicationTests : TestBase
    {
        public GetJobApplicationTests(ITestOutputHelper output)
            : base(output) { }

        [Fact]
        public async Task ShouldReturnDetailsOfSubmittedApplication()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId)
            {
                Title = "The Search"
            };
            var app = new JobApplication(search, "The Organization", "Position Emeritus")
            {
                JobDescription = "Very important role.",
                Location = Location.Toronto,
                Resume = "I am qualified",
                ResumeFormat = Domain.Formats.PLAINTEXT,
            };
            app.Submit(DateTime.Now);
            app.RecordEmployerContact("Seriously?", received: DateTime.Now.AddHours(3));
            await AddAsync(app);

            var query = new GetJobApplicationQuery { JobApplicationId = app.Id };
            var result = await SendAsync(query);

            result.ParentSearchId.ShouldBe(search.Id);
            result.ParentSearchTitle.ShouldBe(search.Title);
            result.LocationCityName.ShouldBe(Location.Toronto.CityName);
            result.LocationProvince.ShouldBe(Location.Toronto.Province);
            result.Resolution.ShouldBe(Domain.ApplicationResolutions.UNRESOLVED);
            result.CurrentStatus.ShouldBe(Domain.ApplicationStatuses.SUBMITTED);
            result.SubmittedOn.ShouldNotBeNull();
            result.SubmittedOn.Value.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }
}
