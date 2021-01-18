using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace LinkedOut.Application.Features.JobSearches.Queries.GetJobSearchesForUser
{
    public class GetJobSearchesForUserQuery : IRequest<JobSearchListVm>
    {
        public string UserId { get; set; }
    }

    public class GetJobSearchesForUserHandler : IRequestHandler<GetJobSearchesForUserQuery, JobSearchListVm>
    {
        private readonly ILogger<GetJobSearchesForUserHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSearchesForUserHandler(ILogger<GetJobSearchesForUserHandler> logger, IApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSearchListVm> Handle(GetJobSearchesForUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching job searches for user {UserId}", request.UserId);

            var searches = await _context
                .JobSearches
                .Where(s => s.OwnerId == request.UserId)
                .ProjectTo<JobSearchSummaryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogDebug("Found {JobSearchCount} searches for user {UserID}", searches.Count, request.UserId);

            return new JobSearchListVm
            {
                JobSearches = searches
            };

        }
    }
}
