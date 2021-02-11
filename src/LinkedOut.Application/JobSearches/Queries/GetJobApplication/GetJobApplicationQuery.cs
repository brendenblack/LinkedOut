using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Queries.GetJobApplication
{
    public class GetJobApplicationQuery : IRequest<JobApplicationDto>
    {
        public int JobApplicationId { get; set; }
    }

    public class GetJobApplicationHandler : IRequestHandler<GetJobApplicationQuery, JobApplicationDto>
    {
        private readonly ILogger<GetJobApplicationHandler> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobApplicationHandler(ILogger<GetJobApplicationHandler> logger, IApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobApplicationDto> Handle(GetJobApplicationQuery request, CancellationToken cancellationToken)
        {
            var dto = await _context.JobApplications
                .Where(a => a.Id == request.JobApplicationId)
                .Include(a => a.Notes)
                .Include(a => a.Transitions)
                .ProjectTo<JobApplicationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();


            return dto;

        }
    }
}
