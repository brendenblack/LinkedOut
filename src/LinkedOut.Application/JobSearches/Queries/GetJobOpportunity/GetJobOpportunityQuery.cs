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

namespace LinkedOut.Application.JobSearches.Queries.GetJobOpportunity
{
    public class GetJobOpportunityQuery : IRequest<JobOpportunityDto>
    {
        public int JobOpportunityId { get; set; }
    }

    public class GetJobOpportunityHandler : IRequestHandler<GetJobOpportunityQuery, JobOpportunityDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobOpportunityHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobOpportunityDto> Handle(GetJobOpportunityQuery request, CancellationToken cancellationToken)
        {

            //var model = await _context.JobApplications
            //    .Where(a => a.Id == request.JobOpportunityId)
            //    .Include(a => a.ParentSearch)
            //    .Include(a => a.Transitions)
            //    .Include(a => a.Notes)
            //    .AsSplitQuery()
            //    .FirstOrDefaultAsync();

            //return _mapper.Map<JobOpportunityDto>(model);

            return await _context.JobApplications
                .Where(a => a.Id == request.JobOpportunityId)
                .Include(a => a.ParentSearch)
                .Include(a => a.Transitions)
                .Include(a => a.Notes)
                .AsSingleQuery() // TODO (improvement): unfortunately query splitting doesn't work on this one
                .AsNoTracking()
                .ProjectTo<JobOpportunityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
