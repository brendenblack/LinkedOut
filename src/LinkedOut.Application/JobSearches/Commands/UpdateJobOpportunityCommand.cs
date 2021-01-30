using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class UpdateJobOpportunityCommand : IRequest<Result>
    {
        public int JobOpportunityId { get; set; }
    }
}
