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
    public class UpdateJobDescriptionCommand : IRequest<Result>
    {
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

        /// <summary>
        /// Whether the job is to be performed primarily remotely.
        /// </summary>
        //public bool IsRemote { get; set; } // TODO: not implemented yet
    }

    public class UpdateJobDescriptionHandler : IRequestHandler<UpdateJobDescriptionCommand, Result>
    {
        private readonly ILogger<UpdateJobDescriptionHandler> _logger;
        private readonly IApplicationDbContext _context;

        public UpdateJobDescriptionHandler(ILogger<UpdateJobDescriptionHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(UpdateJobDescriptionCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .FindAsync(request.JobApplicationId);

            application.OrganizationName = request.OrganizationName;
            application.JobTitle = request.JobTitle;
            //application.Source = request.Source;
            application.JobDescription = request.Description;
            application.JobDescriptionFormat = request.DescriptionFormat;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
