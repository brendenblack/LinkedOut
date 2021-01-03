using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain.Entities
{
    public class StatusTransition
    {
        private StatusTransition() { }

        public StatusTransition(JobApplication application,
                                ApplicationStatuses from,
                                ApplicationStatuses to,
                                DateTime timestamp,
                                ApplicationResolutions resolution)
        {
            ApplicationId = application.Id;
            Application = application;
            Timestamp = timestamp;
            TransitionFrom = from;
            TransitionTo = to;
            Resolution = resolution;
        }

        public int Id { get; set; }

        public int ApplicationId { get; private set; }

        public virtual JobApplication Application { get; private set; }

        public DateTime Timestamp { get; private set; }

        public ApplicationStatuses TransitionTo { get; private set; }

        public ApplicationStatuses TransitionFrom { get; private set; }

        public ApplicationResolutions Resolution { get; private set; }
    }
}