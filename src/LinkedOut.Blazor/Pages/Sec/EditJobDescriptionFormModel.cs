﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Pages.Sec
{
    public class EditJobDescriptionFormModel
    {
        public int? JobApplicationId { get; set; }

        [Required]
        [Display(Name = "job title")]
        public string JobTitle { get; set; } = "Senior Person";

        [Required]
        [Display(Name = "organization name")]
        public string OrganizationName { get; set; } = "Microsoft";

        public string Description { get; set; }

        public bool IsRemote { get; set; } = false;

        public string Source { get; set; }
    }
}