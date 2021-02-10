using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.BlazorWasm.Models
{
    public class EditJobDescriptionFormModel
    {
        public int? JobApplicationId { get; set; }

        [Required]
        [Display(Name = "job title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Organization name")]
        public string OrganizationName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Remote?")]
        public bool IsRemote { get; set; } = false;

        public string Source { get; set; }

        public string City { get; set; }

        public string Province { get; set; }
    }
}
