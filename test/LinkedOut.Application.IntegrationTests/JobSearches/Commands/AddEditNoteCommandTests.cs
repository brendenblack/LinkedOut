using LinkedOut.Application.JobSearches.Commands;
using LinkedOut.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LinkedOut.Application.IntegrationTests.JobSearches.Commands
{
    public class AddEditNoteCommandTests : TestBase
    {
        public AddEditNoteCommandTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public async Task ShouldCreateNote_WhenNoIdProvided()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            await AddAsync(application);
            var command = new AddEditNoteCommand { JobApplicationId = application.Id, Contents = "my note" };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
            var note = await FindAsync<Note>(result.Value);
            note.ShouldNotBeNull();
            note.Contents.ShouldBe("my note");
            note.ApplicationId.ShouldBe(application.Id);
        }

        [Fact]
        public async Task ShouldEditNote_WhenIdProvided()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            var existingNoteResult = application.AddNote("my note");
            await AddAsync(application);
            var command = new AddEditNoteCommand 
            { 
                JobApplicationId = application.Id, 
                NoteId = existingNoteResult.Value.Id, 
                Contents = "my edited note" 
            };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
            var note = await FindAsync<Note>(existingNoteResult.Value.Id);
            note.ShouldNotBeNull();
            note.Contents.ShouldBe("my edited note");
            note.ApplicationId.ShouldBe(application.Id);
        }

    }
}
