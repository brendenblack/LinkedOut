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

namespace LinkedOut.Application.UnitTests.JobSearches.Commands
{
    public class UpdateJobOpportunityCommandTests : CommandTestBase
    {
        private readonly ITestOutputHelper _output;

        public UpdateJobOpportunityCommandTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldOnlyUpdatePosition_WhenOnlyPositionProvided()
        {
            var search = new JobSearch(Guid.NewGuid().ToString());
            var application = new JobApplication(search, "The Organization", "The Position");
            Context.JobApplications.Add(application);
            await Context.SaveChangesAsync(new System.Threading.CancellationToken());
            var logger = new XUnitLogger<UpdateJobOpportunityHandler>(_output);
            var sut = new UpdateJobOpportunityHandler(logger, Context);
            var command = new UpdateJobOpportunityCommand
            {
                JobApplicationId = application.Id,
                JobTitle = "The New Position"
            };

            await sut.Handle(command, new System.Threading.CancellationToken());

            var updatedApplication = Context.JobApplications.First(a => a.Id == application.Id);
            updatedApplication.OrganizationName.ShouldBe("The Organization");
            updatedApplication.JobTitle.ShouldBe("The New Position");

        }
    }
}
