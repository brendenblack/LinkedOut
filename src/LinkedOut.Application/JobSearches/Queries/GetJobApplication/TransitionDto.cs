using LinkedOut.Application.Common.Mappings;
using LinkedOut.Domain;
using LinkedOut.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Queries.GetJobApplication
{
    public class TransitionDto : IMapFrom<StatusTransition>
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public ApplicationStatuses TransitionTo { get; private set; }

        public ApplicationStatuses TransitionFrom { get; private set; }

        public ApplicationResolutions Resolution { get; private set; }
    }
}
