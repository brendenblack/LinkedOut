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
    public class TransitionApplicationCommandTests : TestBase
    {
        public TransitionApplicationCommandTests(ITestOutputHelper output) 
            : base(output) { }

        #region Submit tests
        [Fact]
        public async Task SubmitShouldTransition_WhenInProgress()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            await AddAsync(application);
            var command = new TransitionApplicationCommand { JobApplicationId = application.Id, Action = JobApplicationAction.SUBMISSION };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
            JobApplication modifiedApplication;
            using (var db = GetContext())
            {
                modifiedApplication = await db.JobApplications
                    .Include(a => a.Transitions)
                    .FirstAsync(a => a.Id == application.Id);
            }

            modifiedApplication.CurrentStatus.ShouldBe(ApplicationStatuses.SUBMITTED);
        }

        [Fact]
        public async Task SubmitShouldFail_WhenSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            application.Submit(DateTime.Now);
            await AddAsync(application);
            var command = new TransitionApplicationCommand { JobApplicationId = application.Id, Action = JobApplicationAction.SUBMISSION };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeFalse();
        }

        [Fact]
        public async Task SubmitShouldFail_WhenClosed()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            application.Submit(DateTime.Now);
            application.Withdraw(DateTime.Now);
            await AddAsync(application);
            var command = new TransitionApplicationCommand { JobApplicationId = application.Id, Action = JobApplicationAction.SUBMISSION };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeFalse();
        }
        #endregion

        #region Withdraw
        [Fact]
        public async Task WithdrawShouldTransitionAndResolve_WhenSubmitted()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            application.Submit(DateTime.Now);
            await AddAsync(application);
            var command = new TransitionApplicationCommand { JobApplicationId = application.Id, Action = JobApplicationAction.WITHDRAWAL };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeTrue();
            JobApplication modifiedApplication;
            using (var db = GetContext())
            {
                modifiedApplication = await db.JobApplications
                    .Include(a => a.Transitions)
                    .FirstAsync(a => a.Id == application.Id);
            }

            modifiedApplication.CurrentStatus.ShouldBe(ApplicationStatuses.CLOSED);
            modifiedApplication.Resolution.ShouldBe(ApplicationResolutions.WITHDRAWN);
        }

        [Fact]
        public async Task WithdrawShouldFail_WhenClosed()
        {
            var userId = await RunAsDefaultUserAsync();
            var search = new JobSearch(userId);
            var application = new JobApplication(search, "", "");
            application.Submit(DateTime.Now);
            application.Withdraw(DateTime.Now);
            await AddAsync(application);
            var command = new TransitionApplicationCommand { JobApplicationId = application.Id, Action = JobApplicationAction.WITHDRAWAL };

            var result = await SendAsync(command);

            result.IsSuccess.ShouldBeFalse();
        }

        #endregion
    }
}
