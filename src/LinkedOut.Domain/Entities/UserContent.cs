using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Domain.Entities
{
    public class UserContent
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public Formats Format { get; set; }
    }
}
