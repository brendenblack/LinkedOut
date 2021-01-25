using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class SubmitApplicationCommand : IRequest<Result>
    {
        public int JobApplicationId { get; set; }

        public string ResumeContents { get; set; }

        public Formats ResumeFormat { get; set; }

        public string CoverLetterContents { get; set; }

        public Formats CoverLetterFormat { get; set; } = Formats.HTML;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class SubmitApplicationHandler : IRequestHandler<SubmitApplicationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<SubmitApplicationHandler> _logger;

        public SubmitApplicationHandler(IApplicationDbContext context, ILogger<SubmitApplicationHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .Where(a => a.Id == request.JobApplicationId)
                .Include(a => a.Transitions)
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            application.Submit(request.Timestamp, request.ResumeContents, request.ResumeFormat);
            application.CoverLetter = request.CoverLetterContents;
            // application.CoverLetterFormat = request.CoverLetterFormat;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
