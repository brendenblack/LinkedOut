using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Queries.GetJobOpportunity
{
    public class TransitionDto : IMapFrom<StatusTransition>
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public ApplicationStatuses TransitionTo { get; set; }

        public ApplicationStatuses TransitionFrom { get; set; }

        public ApplicationResolutions Resolution { get; set; }
    }
}
