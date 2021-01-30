using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Shared
{
    public class EmployerContactFormModel
    {
        public int? NoteId { get; set; }

        public int? JobApplicationId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string Author { get; set; }
    }
}
