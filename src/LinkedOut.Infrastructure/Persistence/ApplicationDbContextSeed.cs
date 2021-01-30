using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.Models;
using LinkedOut.Domain.ValueObjects;
using LinkedOut.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedTestData(IApplicationDbContext context, string seedUserId)
        {
            if (!context.JobSearches.Any())
            {
                //var search = new JobSearch(seedUserId) { Title = "The Big Seek" };
                //context.JobSearches.Add(search);

                //var resumeContents = "References:<ol><li><i>available</i></li><li>upon</li><li>request</li></ol>";

                //var application1 = new JobApplication(search, "Microsoft", "Junior Developer")
                //{
                //    JobDescription = "Try not to break Windows.<br />Must <strong>love</strong>: <ul><li>C#</li></ul><br>We value diversity.",
                //    JobDescriptionFormat = Formats.HTML,
                //    CoverLetter = "Hello, I would like a job. Please see my resume.",
                //    Location = Location.Toronto,
                //    Resume = resumeContents,
                //    ResumeFormat = Formats.HTML,
                //};
                //application1.Submit(DateTime.Now);
                ////new TransitionManager(application1).Submit();
                //application1.RecordEmployerContact("Hi, we have received your application and if we can be bothered we'll be in touch.", "Jane Doe", "Application received");
                //context.JobApplications.Add(application1);

                //var application2 = new JobApplication(search, "Facebook", "Junior Stooge #3")
                //{
                //    JobDescription = "We need someone to fix our facial recognition algorithm to be able to identity minorities.",
                //    JobDescriptionFormat = Formats.HTML,
                //    CoverLetter = "Hello, I would like a job. Please see my resume.",
                //    Location = Location.Remote,
                //    Resume = resumeContents,
                //    ResumeFormat = Formats.HTML
                //};
                //var manager2 = new TransitionManager(application2);
                //manager2.Submit();
                //application2.RecordEmployerContact("Hi, we have received your application and if we can be bothered we'll be in touch.", "Tim Apple", "Application received");
                //manager2.Close(ApplicationResolutions.REJECTED);
                //application2.RecordEmployerContact("No thanks. Don't call us.", "Tim Apple", "Your rejection");
                //context.JobApplications.Add(application2);

                //await context.SaveChangesAsync(new System.Threading.CancellationToken());
            }
        }
    }
}
