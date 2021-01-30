using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.Models;
using LinkedOut.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Infrastructure.Services
{
    public class FuturamaNewUserService : INewUserService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<FuturamaNewUserService> _logger;

        public FuturamaNewUserService(IApplicationDbContext context, ILogger<FuturamaNewUserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeNewUser(string userid)
        {
            _logger.LogInformation("Populating data for user {UserId}", userid);

            if (_context.JobSearches.Any(s => s.OwnerId == userid))
            {
                _logger.LogWarning("User {UserId} already has populated data; skipping initialization");
                return;
            }

            var search = new JobSearch(userid)
            {
                Title = "Job Search 3000",
                Description = "Welcome to the job market of tomorrow!",
            };
            
            var cryogenics = new JobApplication(search, "Applied Cryogenics", "Lab Tech")
            {
                JobDescription = "Take care of people who have been frozen.",
                JobDescriptionFormat = Formats.HTML,
                Location = new Location("New York", "New York"),
            };
            cryogenics.Submit(DateTime.Now);
            cryogenics.RecordEmployerContact("Come, your destiny awaits!", "I. C. Weiner");
            new TransitionManager(cryogenics).Close(ApplicationResolutions.REJECTED);
            _context.JobApplications.Add(cryogenics);

            var deliveryBoy = new JobApplication(search, "Planet Express", "Delivery Boy")
            {
                JobDescription = "<p>You will be responsible for ensuring that the cargo reaches its destination.</p>",
                JobDescriptionFormat = Formats.HTML,
                Resume = "<h1>Work History</h1><h2>Delivery Boy - Panucci's Pizza</h2><h3>New York, New York 1997-1999</h3><ul><li>Delivered pizza</li></ul>",
                ResumeFormat = Formats.HTML,
                Location = new Location("New New York", "New New York")
            };
            deliveryBoy.AddNote("Maybe in this job I can fly through space fighting monsters and teaching alien women to <i>lurve</i>");
            _context.JobApplications.Add(deliveryBoy);          

            await _context.SaveChangesAsync(new CancellationToken());

            return;
        }
    }
}
