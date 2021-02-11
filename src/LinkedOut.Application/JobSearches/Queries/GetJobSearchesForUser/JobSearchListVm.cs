using System.Collections.Generic;

namespace LinkedOut.Application.JobSearches.Queries.GetJobSearchesForUser
{
    public class JobSearchListVm
    {
        public List<JobSearchSummaryDto> JobSearches { get; set; } = new ();
    }
}