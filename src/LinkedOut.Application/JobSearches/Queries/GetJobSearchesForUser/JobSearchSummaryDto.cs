using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LinkedOut.Application.JobSearches.Queries.GetJobSearchesForUser
{
    public class JobSearchSummaryDto : IMapFrom<JobSearch>
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Created")]
        public DateTime Created { get; set; }

        [DisplayName("# of applications")]
        public int ApplicationsCount { get; set; }
    }
}
