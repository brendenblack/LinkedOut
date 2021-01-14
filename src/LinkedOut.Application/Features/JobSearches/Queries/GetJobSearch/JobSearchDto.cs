using AutoMapper;
using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.Features.JobSearches.Queries.GetJobSearch
{
    public class JobSearchDto : IMapFrom<JobSearch>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public bool IsActive { get; set; }

        public void MapFrom(Profile profile)
        {
            profile.CreateMap<JobSearch, JobSearchDto>()
                .ForMember(s => s.IsActive, opt => opt.Ignore());
        }
    }
}
