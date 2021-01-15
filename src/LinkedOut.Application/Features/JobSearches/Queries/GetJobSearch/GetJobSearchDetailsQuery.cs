using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.Features.JobSearches.Queries.GetJobSearch
{
    public class GetJobSearchDetailsQuery : IRequest<JobSearchDto>
    {
        public int JobSearchId { get; set; }
    }

    public class GetJobSearchDetailsHandler : IRequestHandler<GetJobSearchDetailsQuery, JobSearchDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSearchDetailsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSearchDto> Handle(GetJobSearchDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _context.JobSearches
                .Where(s => s.Id == request.JobSearchId)
                .ProjectTo<JobSearchDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
