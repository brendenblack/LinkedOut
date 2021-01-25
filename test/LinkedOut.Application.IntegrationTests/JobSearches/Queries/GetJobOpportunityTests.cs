using LinkedOut.Application.JobSearches.Queries.GetJobOpportunity;
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

        #region Status tests
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

        #region Note tests
        [Fact]
        public async Task ShouldIncludeNoteAndIdentifySelfAuthored()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            application.RecordEmployerContact("You'll be responsible for ensuring that the cargo reaches its destination.", "Professor Farnsworth");
            application.AddNote("Alright! I'm a delivery boy!");
            await AddAsync(application);
            var query = new GetJobOpportunityQuery { JobOpportunityId = application.Id };

            var result = await SendAsync(query);

            var selfAuthoredNote = result.Notes.FirstOrDefault(n => n.IsSelfAuthored);
            selfAuthoredNote.ShouldNotBeNull();
            selfAuthoredNote.Author.ShouldBe(Note.SelfAuthoredAuthor);
            selfAuthoredNote.Contents.ShouldBe("Alright! I'm a delivery boy!");
            var contact = result.Notes.FirstOrDefault(n => n.Author == "Professor Farnsworth");
            contact.ShouldNotBeNull();
            contact.Contents.ShouldBe("You'll be responsible for ensuring that the cargo reaches its destination.");
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

            result.Location.ShouldBe("Parts Unknown");
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

            result.Location.ShouldBe(Location.Toronto.ToString());
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

            result.Location.ShouldBe("New New York");
        }
        #endregion
    }
}
