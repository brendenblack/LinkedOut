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

namespace LinkedOut.Application.JobSearches.Queries.GetJobSearch
{
    public class GetJobSearchDetailsQuery : IRequest<JobSearchDto>
    {
        public int JobSearchId { get; set; }
    }

    public class GetJobSearchDetailsHandler : IRequestHandler<GetJobSearchDetailsQuery, JobSearchDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSearchDetailsHandler( IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSearchDto> Handle(GetJobSearchDetailsQuery request, CancellationToken cancellationToken)
        {
            var model = await _context.JobSearches
                .Where(s => s.Id == request.JobSearchId)
                .Include(s => s.Applications)
                    .ThenInclude(a => a.Transitions)
                .Include(s => s.Applications)
                    .ThenInclude(a => a.Notes)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            var dto = _mapper.Map<JobSearchDto>(model);
            return dto;

            // It seems like this query and mapping were too complex, I wasn't able to debug why the below doesn't work 

            //return await _context.JobSearches
            //    .Where(s => s.Id == request.JobSearchId)
            //    .Include(s => s.Applications)
            //        .ThenInclude(a => a.Transitions)
            //    .Include(s => s.Applications)
            //        .ThenInclude(a => a.Notes)
            //    .AsSplitQuery()
            //    .ProjectTo<JobSearchDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync();
        }
    }
}
