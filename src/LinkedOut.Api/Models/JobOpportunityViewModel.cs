using LinkedOut.Application.JobSearches.Queries.GetJobApplication;
using LinkedOut.Application.JobSearches.Queries.GetJobOpportunity;

namespace LinkedOut.Api.Models
{
    public class JobOpportunityViewModel
    {
        public JobOpportunityDto JobOpportunity { get; set; }

        public JobApplicationDto JobApplication { get; set; }
    }
}
