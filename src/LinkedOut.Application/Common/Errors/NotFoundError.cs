using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message)
            : base(message) { }

        public NotFoundError(string name, object key)
            : base($"Unable to find the requested item") 
        {
            Metadata.Add("Entity type", name);
            Metadata.Add("Entity Id", key);
        }
    }
}
