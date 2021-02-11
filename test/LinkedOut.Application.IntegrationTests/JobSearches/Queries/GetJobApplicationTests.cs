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
                ResumeFormat = Formats.PLAINTEXT,
            };
            app.Submit(DateTime.Now);
            app.RecordEmployerContact("Seriously?", received: DateTime.Now.AddHours(3));
            await AddAsync(app);

            var query = new GetJobApplicationQuery { JobApplicationId = app.Id };
            var result = await SendAsync(query);

            result.ParentSearchId.ShouldBe(search.Id);
            result.Resolution.ShouldBe(ApplicationResolutions.UNRESOLVED);
            result.CurrentStatus.ShouldBe(ApplicationStatuses.SUBMITTED);
            result.SubmittedOn.ShouldNotBeNull();
            result.SubmittedOn.Value.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        #region Resolution
        [Fact]
        public async Task ResolutionShouldBeUnresolved_WhenNotSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var app = new JobApplication(search, "Planet Express", "Delivery Boy");
            await AddAsync(app);
            var query = new GetJobApplicationQuery { JobApplicationId = app.Id };

            var sut = await SendAsync(query);

            sut.Resolution.ShouldBe(ApplicationResolutions.UNRESOLVED);
        }

        [Fact]
        public async Task ResolutionShouldBeUnresolved_WhenSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var app = new JobApplication(search, "Planet Express", "Delivery Boy");
            app.Submit(DateTime.Now);
            await AddAsync(app);
            var query = new GetJobApplicationQuery { JobApplicationId = app.Id };

            var sut = await SendAsync(query);

            sut.Resolution.ShouldBe(ApplicationResolutions.UNRESOLVED);
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
            var query = new GetJobApplicationQuery { JobApplicationId = application.Id };

            var result = await SendAsync(query);

            var selfAuthoredNote = result.Notes.FirstOrDefault(n => n.IsSelfAuthored);
            selfAuthoredNote.ShouldNotBeNull();
            selfAuthoredNote.Author.ShouldBe(Note.SelfAuthoredAuthor);
            selfAuthoredNote.Contents.ShouldBe("Alright! I'm a delivery boy!");
            var contact = result.Notes.FirstOrDefault(n => n.Author == "Professor Farnsworth");
            contact.ShouldNotBeNull();
            contact.Contents.ShouldBe("You'll be responsible for ensuring that the cargo reaches its destination.");
        }

        [Fact]
        public async Task NotesShouldBeEmptyList_WhenNoNotes()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "Planet Express", "Delivery Boy");
            await AddAsync(application);
            var query = new GetJobApplicationQuery { JobApplicationId = application.Id };

            var result = await SendAsync(query);

            result.Notes.ShouldNotBeNull();
            result.Notes.ShouldBeEmpty();
        }
        #endregion
    }
}
