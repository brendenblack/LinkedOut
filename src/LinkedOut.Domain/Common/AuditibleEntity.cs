using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Indicates whether this record is active or not, in support of logical deletion.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
