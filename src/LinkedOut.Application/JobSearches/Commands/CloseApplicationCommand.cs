using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class CloseApplicationCommand : IRequest<Result>
    {
        public int JobApplicationId { get; set; }
    }

    public class CloseApplicationHandler : IRequestHandler<CloseApplicationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CloseApplicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CloseApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .Include(a => a.Transitions)
                .Where(a => a.Id == request.JobApplicationId)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                return Result.Fail(new NotFoundError(nameof(application), request.JobApplicationId));
            }

            if (application.DidApply)
            {
                new TransitionManager(application).Close(ApplicationResolutions.WITHDRAWN);
            }
            else
            {
                new TransitionManager(application).Close(ApplicationResolutions.CANCELLED);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
