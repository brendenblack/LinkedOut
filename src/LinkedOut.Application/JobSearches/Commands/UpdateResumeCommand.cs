using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class UpdateResumeCommand : IRequest<Result>
    {
        public int JobApplicationId { get; set; }
        
        public string Contents { get; set; }

        public Formats Format { get; set; }
    }

    public class UpdateResumeHandler : IRequestHandler<UpdateResumeCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateResumeHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateResumeCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications.FindAsync(request.JobApplicationId);

            application.Resume = request.Contents;
            application.ResumeFormat = request.Format;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
