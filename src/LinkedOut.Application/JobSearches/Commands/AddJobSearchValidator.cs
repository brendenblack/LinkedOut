using FluentValidation;
using LinkedOut.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class AddJobSearchValidator : AbstractValidator<AddJobSearchCommand>
    {
        public AddJobSearchValidator(IApplicationDbContext context)
        {
        }
    }
}
