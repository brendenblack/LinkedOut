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
    public class DeleteJobSearchCommand : IRequest<Result>
    {
        public int JobSearchId { get; set; }
    }

    public class DeleteJobSearchCommandHandler : IRequestHandler<DeleteJobSearchCommand, Result>
    {
        private readonly ILogger<DeleteJobSearchCommandHandler> _logger;
        private readonly IApplicationDbContext _context;

        public DeleteJobSearchCommandHandler(ILogger<DeleteJobSearchCommandHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(DeleteJobSearchCommand request, CancellationToken cancellationToken)
        {
            var search = await _context.JobSearches
                .FirstOrDefaultAsync(s => s.Id == request.JobSearchId);

            if (search == null)
            {
                return Result.Fail(new NotFoundError(nameof(JobSearch), request.JobSearchId));
            }

            _context.JobSearches.Remove(search);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "An exception occurred while trying to delete job search with id {JobSearchId}", request.JobSearchId);
                return Result.Fail(e.Message);
            }
        }
    }
}
