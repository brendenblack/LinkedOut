using LinkedOut.Application.JobSearches.Commands;
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
    public class AddJobSearchCommandTests : TestBase
    {
        public AddJobSearchCommandTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ShouldReturnIdOnSuccess()
        {
            var userId = await RunAsDefaultUserAsync();
            var command = new AddJobSearchCommand
            {
                CreatedOn = DateTime.Now,
                OwnerId = userId,
                Title = "The Search"
            };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBeGreaterThan(0);
        }

    }
}
