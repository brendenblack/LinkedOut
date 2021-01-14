using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain.Entities;
using System;
using System.ComponentModel;

namespace LinkedOut.Application.Features.JobSearches.Queries.GetJobSearchesForUser
{
    public class JobSearchSummaryDto : IMapFrom<JobSearch>
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("# of applications")]
        public int ApplicationsCount { get; set; }

        [DisplayName("Created")]
        public DateTime Created { get; set; }
    }
}
