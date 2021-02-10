using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.BlazorWasm.Models
{
    public class CreateJobSearchModel
    {
        [Required(ErrorMessage = "Every search needs a title")]
        [MinLength(3)]
        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime Start { get; set; } = DateTime.Now;
    }
}
