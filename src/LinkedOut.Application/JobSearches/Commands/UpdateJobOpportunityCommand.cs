using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    /// <summary>
    /// Allows the updating of details for a specific job posting.
    /// </summary>
    public class UpdateJobOpportunityCommand : IRequest<Result>
    {
        /// <summary>
        /// The ID of the application to update.
        /// </summary>
        /// <remarks>Required.</remarks>
        public int JobApplicationId { get; set; }

        /// <summary>
        /// The target for the organization name. Set to the existing value to preserve it.
        /// </summary>
        public string? OrganizationName { get; set; } = null;

        /// <summary>
        /// The target for the job title. Set to the existing value to preserve it.
        /// </summary>
        public string JobTitle { get; set; } = null;

        /// <summary>
        /// Where this opportunity was discovered. Set to the existing value to preserve it.
        /// </summary>
        public string Source { get; set; } = null;

        /// <summary>
        /// A description of the company, position, roles, responsibilities, etc.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// What format the <see cref="Description"/> contents should be rendered as.
        /// </summary>
        public Formats? DescriptionFormat { get; set; } = Formats.PLAINTEXT;

        /// <summary>
        /// What city the job is centred in. 
        /// </summary>
        /// <remarks>
        /// If the job is remote, this would be the home base of the position or company headquarters.
        /// </remarks>
        public string LocationCityName { get; set; } = null;

        /// <summary>
        /// What province the job is centred in. 
        /// </summary>
        /// <remarks>
        /// If the job is remote, this would be the home base of the position or company headquarters.
        /// </remarks>
        public string LocationProvince { get; set; } = null;

        /// <summary>
        /// Whether the job is to be performed primarily remotely.
        /// </summary>
        public bool? IsRemote { get; set; } // TODO: not implemented yet
    }

    public class UpdateJobOpportunityHandler : IRequestHandler<UpdateJobOpportunityCommand, Result>
    {
        private readonly ILogger<UpdateJobOpportunityHandler> _logger;
        private readonly IApplicationDbContext _context;

        public UpdateJobOpportunityHandler(ILogger<UpdateJobOpportunityHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(UpdateJobOpportunityCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .FindAsync(request.JobApplicationId);

            if (!string.IsNullOrWhiteSpace(request.OrganizationName))
            {
                application.OrganizationName = request.OrganizationName;
            }
            
            if (!string.IsNullOrWhiteSpace(request.JobTitle))
            {
                application.JobTitle = request.JobTitle;
            }

            if (request.Description is not null)
            {
                application.JobDescription = request.Description;
                application.JobDescriptionFormat = request.DescriptionFormat ?? Formats.HTML;
            }
            
            
            //application.Source = request.Source;
            

            if (request.IsRemote.HasValue)
            {
                // TODO
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
