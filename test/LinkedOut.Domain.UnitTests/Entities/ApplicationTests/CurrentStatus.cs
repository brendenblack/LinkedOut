using LinkedOut.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinkedOut.Domain.UnitTests.Entities.ApplicationTests
{
    public class CurrentStatus
    {
        [Fact]
        public void NewlyCreated_ShouldReturn_INPROGRESS()
        {
            var search = new JobSearch();
            var app = new JobApplication(search, "", "");

            app.CurrentStatus.ShouldBe(ApplicationStatuses.INPROGRESS);
        }
    }
}
