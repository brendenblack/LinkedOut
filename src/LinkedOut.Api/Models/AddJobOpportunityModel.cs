using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Api.Models
{
    public class AddJobOpportunityModel
    {
        [Required(ErrorMessage = "An organization name is required.")]
        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "A job title is required.")]
        [Display(Name = "Job title")]
        public string JobTitle { get; set; }

        public string Description { get; set; } = "";

        public bool IsRemote { get; set; } = false;

        public string Source { get; set; }

        public string City { get; set; }

        public string Province { get; set; }
    }
}
