using LinkedOut.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain.Entities
{
    public class JobSearch : AuditableEntity
    {
        private JobSearch() { }

        public JobSearch(string ownerId)
        {
            OwnerId = ownerId;
        }

        public int Id { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<JobApplication> Applications { get; private set; } = new List<JobApplication>();

        public string OwnerId { get; private set; }
    }
}
