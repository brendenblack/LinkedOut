using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using LinkedOut.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    /// <summary>
    /// Creates a record of a job posting that the user is considering applying for during a
    /// specified <see cref="JobSearch"/>.
    /// </summary>
    public class AddJobDescriptionCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// The <see cref="JobSearch"/> that this job opportunity is being considered for.
        /// </summary>
        public int JobSearchId { get; set; }

        /// <summary>
        /// The ID of the application to update.
        /// </summary>
        /// <remarks>Required.</remarks>
        public int JobApplicationId { get; set; }

        /// <summary>
        /// The target for the organization name. Set to the existing value to preserve it.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// The target for the job title. Set to the existing value to preserve it.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Where this opportunity was discovered. Set to the existing value to preserve it.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// A description of the company, position, roles, responsibilities, etc.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// What format the <see cref="Description"/> contents should be rendered as.
        /// </summary>
        public Formats DescriptionFormat { get; set; } = Formats.PLAINTEXT;

        public bool IsRemote { get; set; }

        /// <summary>
        /// What city the job is centred in. 
        /// </summary>
        /// <remarks>
        /// If the job is remote, this would be the home base of the position or company headquarters.
        /// </remarks>
        public string LocationCityName { get; set; }

        /// <summary>
        /// What province the job is centred in. 
        /// </summary>
        /// <remarks>
        /// If the job is remote, this would be the home base of the position or company headquarters.
        /// </remarks>
        public string LocationProvince { get; set; }
    }

    public class AddJobDescriptionHandler : IRequestHandler<AddJobDescriptionCommand, Result<int>>
    {
        private readonly ILogger<AddJobDescriptionHandler> _logger;
        private readonly IApplicationDbContext _context;

        public AddJobDescriptionHandler(ILogger<AddJobDescriptionHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<int>> Handle(AddJobDescriptionCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.JobSearches.FindAsync(request.JobSearchId);

            _logger.LogDebug("Adding a job of {JobTitle} at {Organization} to job search {JobSearchId}",
                request.JobTitle,
                request.OrganizationName,
                parent.Id);

            JobApplication application = new JobApplication(parent, request.OrganizationName, request.JobTitle)
            {
                Source = request.Source,
                JobDescription = request.Description,
                JobDescriptionFormat = request.DescriptionFormat,
                Location = (request.IsRemote) ? Location.Remote : new Location(request.LocationCityName, request.LocationProvince),
            };

            _context.JobApplications.Add(application);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("Created job description: {@Application}", application);

            return Result.Ok(application.Id);
        }
    }
}
