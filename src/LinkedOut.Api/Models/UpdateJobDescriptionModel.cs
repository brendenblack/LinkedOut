using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Api.Models
{
    public class UpdateJobDescriptionModel
    {
        public string Description { get; set; }

        public Formats Format { get; set; }
    }
}
