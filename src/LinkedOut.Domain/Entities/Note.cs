using LinkedOut.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain.Entities
{
    public class Note : AuditableEntity
    {
        protected Note() { }

        public Note(JobApplication application)
        {
            this.Application = application;
            this.ApplicationId = application.Id;
        }

        public int Id { get; private set; }

        public int ApplicationId { get; private set; }

        public virtual JobApplication Application { get; private set; }

        public DateTime Timestamp { get; set; }

        public string Subject { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }

        public bool IsSelfAuthored => Author == SelfAuthoredAuthor;

        public static readonly string SelfAuthoredAuthor = "self";
    }
}
