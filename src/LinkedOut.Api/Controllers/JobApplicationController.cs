using LinkedOut.Api.Models;
using LinkedOut.Application.JobSearches.Commands;
using LinkedOut.Application.JobSearches.Queries.GetJobApplication;
using LinkedOut.Application.JobSearches.Queries.GetJobOpportunity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ApiController
    {
        [HttpGet("{jobApplicationId:int}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplication([FromRoute] int jobApplicationId)
        {
            return await Mediator.Send(new GetJobApplicationQuery { JobApplicationId = jobApplicationId });
        }

        [HttpPost("{jobApplicationId:int}/status")]
        public async Task<ActionResult> TransitionApplication([FromRoute] int jobApplicationId, [FromBody] ApplicationActions action)
        {
            var command = new TransitionApplicationCommand
            {
                JobApplicationId = jobApplicationId,
                Action = action
            };

            var result = await Mediator.Send(command);

            return (result.IsSuccess) ? NoContent() : HandleFailureResult(result);
        }
    }
}
