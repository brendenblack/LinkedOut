using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Entities;
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
    public class AddJobSearchCommand : IRequest<Result<int>>
    {
        public string OwnerId { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class AddJobSearchHandler : IRequestHandler<AddJobSearchCommand, Result<int>>
    {
        private readonly ILogger<AddJobSearchHandler> _logger;
        private readonly IApplicationDbContext _context;

        public AddJobSearchHandler(ILogger<AddJobSearchHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<int>> Handle(AddJobSearchCommand request, CancellationToken cancellationToken)
        {
            var jobSearch = new JobSearch(request.OwnerId)
            {
                Title = request.Title,
                Created = request.CreatedOn
            };

            _context.JobSearches.Add(jobSearch);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(jobSearch.Id);
        }
    }
}
