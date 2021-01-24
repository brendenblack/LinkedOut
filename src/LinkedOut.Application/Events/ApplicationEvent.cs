using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.Events
{
    public abstract class ApplicationEvent
    {

        public DateTime IsPublished { get; set; }

        public DateTimeOffset Occurred { get; protected set; } = DateTime.UtcNow;
    }
}
