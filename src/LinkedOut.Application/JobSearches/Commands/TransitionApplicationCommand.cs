using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Entities;
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
    public class TransitionApplicationCommand : IRequest<Result>
    {
        public int JobApplicationId { get; set; }

        public JobApplicationAction Action { get; set; }
    }

    public enum JobApplicationAction
    {
        SUBMIT,
        WITHDRAW,
        ACCEPT_OFFER,
        REJECT_OFFER,
        REOPEN,
    }

    public class TransitionApplicationHandler : IRequestHandler<TransitionApplicationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<TransitionApplicationHandler> _logger;
        private readonly IDateTime _dateTime;

        public TransitionApplicationHandler(
            IApplicationDbContext context, 
            ILogger<TransitionApplicationHandler> logger, 
            IDateTime dateTime)
        {
            _context = context;
            _logger = logger;
            _dateTime = dateTime;
        }

        public async Task<Result> Handle(TransitionApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .Where(a => a.Id == request.JobApplicationId)
                .Include(a => a.Transitions)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                return Result.Fail(new NotFoundError(nameof(application), request.JobApplicationId));
            }

            Result<StatusTransition> transitionResult = null;
            switch (request.Action)
            {
                case JobApplicationAction.SUBMIT:
                    transitionResult = application.Submit(_dateTime.Now);
                    break;
                case JobApplicationAction.WITHDRAW:
                    transitionResult = application.Withdraw(_dateTime.Now);
                    break;
                case JobApplicationAction.REOPEN:
                    transitionResult = application.Reopen(_dateTime.Now);
                    break;
                case JobApplicationAction.ACCEPT_OFFER:
                    throw new NotImplementedException();
                case JobApplicationAction.REJECT_OFFER:
                    throw new NotImplementedException();
            }

            await _context.SaveChangesAsync(cancellationToken);

            return transitionResult.IsSuccess ? Result.Ok() : transitionResult.ToResult();
        }
    }
}
