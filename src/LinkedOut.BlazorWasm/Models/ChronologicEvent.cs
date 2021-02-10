using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.BlazorWasm.Models
{
    public class ChronologicEvent
    {
        public DateTime Timestamp { get; set; }

        public object Value { get; set; }

        public Type Type { get; set; }
    }
}
