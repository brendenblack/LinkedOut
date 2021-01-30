using LinkedOut.Application.JobSearches.Commands;
using LinkedOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
    public class DeleteNoteCommandTests : TestBase
    {
        public DeleteNoteCommandTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public async Task ShouldFailWhenNotOwner()
        {
            await RunAsDefaultUserAsync();
            var search = new JobSearch("someone-else-entirely");
            var application = new JobApplication(search, "somewhere", "something");
            var note = application.AddNote("this isn't my note");
            await AddAsync(application);
            var command = new DeleteNoteCommand { NoteId = note.Value.Id };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeFalse();
        }
        
        [Fact]
        public async Task ShouldSucceedWhenOwner()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "somewhere", "something");
            var note = application.AddNote("this is my note");
            await AddAsync(application);
            var command = new DeleteNoteCommand { NoteId = note.Value.Id };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ShouldLogicallyDelete()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "somewhere", "something");
            var note = application.AddNote("this is my note");
            await AddAsync(application);
            var command = new DeleteNoteCommand { NoteId = note.Value.Id };

            var result = await SendAsync(command);

            var deletedNote = await FindAsync<Note>(note.Value.Id);
            bool logicallyExists = false;
            using (var context = GetContext())
            {
                logicallyExists = context.Notes
                    .IgnoreQueryFilters()
                    .Any(n => n.Id == note.Value.Id);                
            }

            deletedNote.ShouldBeNull();
            logicallyExists.ShouldBeTrue();

        }
    }
}
