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
    public class UpdateCoverLetterCommand : IRequest<Result>
    {
        public int JobApplicationId { get; set; }
        
        public string Contents { get; set; }

        //public Formats Format { get; set; }
    }

    public class UpdateCoverLetterHandler : IRequestHandler<UpdateCoverLetterCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCoverLetterHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateCoverLetterCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications.FindAsync(request.JobApplicationId, cancellationToken);

            application.CoverLetter = request.Contents;
            //application.CoverLetter = request.Format;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
