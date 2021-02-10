using FluentResults;
using LinkedOut.Api.Models;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Application.JobSearches.Commands;
using LinkedOut.Application.JobSearches.Queries.GetJobApplication;
using LinkedOut.Application.JobSearches.Queries.GetJobOpportunity;
using LinkedOut.Application.JobSearches.Queries.GetJobSearch;
using LinkedOut.Application.JobSearches.Queries.GetJobSearchesForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Api.Controllers
{
    public class JobSearchController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public JobSearchController(ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        [HttpGet]
        public async Task<JobSearchListVm> GetJobSearchesForUser()
        {
            var query = new GetJobSearchesForUserQuery
            {
                UserId = _currentUserService.UserId
            };

            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddJobSearch([FromBody] string title)
        {
            var command = new AddJobSearchCommand
            {
                CreatedOn = _dateTime.Now,
                Title = title,
                OwnerId = _currentUserService.UserId
            };

            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetJobSearch), new { jobSearchId = result.Value }, result.Value);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Reasons);
            }


        }
        
        [HttpGet("{jobSearchId:int}")]
        public async Task<ActionResult<JobSearchDto>> GetJobSearch([FromRoute] int jobSearchId)
        {
            var query = new GetJobSearchDetailsQuery
            {
                JobSearchId = jobSearchId
            };

            return await Mediator.Send(query);
        }
        
        [HttpDelete("{jobSearchId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteJobSearch([FromRoute] int jobSearchId)
        {
            var command = new DeleteJobSearchCommand { JobSearchId = jobSearchId };
            var result = await Mediator.Send(command);

            return (result.IsSuccess) 
                ? NoContent() 
                : HandleFailureResult(result);
        }

        [HttpPost("{jobSearchId:int}/opportunity")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> AddJobOpportunity([FromRoute] int jobSearchId, [FromBody] AddJobOpportunityModel model)
        {
            var command = new AddJobOpportunityCommand
            {
                JobSearchId = jobSearchId,
                OrganizationName = model.OrganizationName,
                JobTitle = model.JobTitle,
                LocationCityName = model.City,
                LocationProvince = model.Province,
                IsRemote = model.IsRemote,
                Description = model.Description,
                DescriptionFormat = Formats.HTML, // TODO
                Source = model.Source
            };

            var result = await Mediator.Send(command);

            return (result.IsSuccess) 
                ? CreatedAtAction(nameof(GetJobOpportunity), new { jobSearchId = jobSearchId, jobOpportunityId = result.Value }, result.Value)
                : HandleFailureResult(result);

        }

        [HttpGet("{jobSearchId:int}/opportunity/{jobOpportunityId:int}")]
        public async Task<ActionResult<JobOpportunityDto>> GetJobOpportunity([FromRoute] int jobSearchId, [FromRoute] int jobOpportunityId)
        {
            return await Mediator.Send(new GetJobOpportunityQuery { JobOpportunityId = jobOpportunityId });
        }

        [HttpPut("{jobSearchId:int}/opportunity/{jobOpportunityId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateJobDescription([FromRoute] int jobSearchId, [FromRoute] int jobOpportunityId, [FromBody] UpdateJobDescriptionModel model)
        {
            var command = new UpdateJobOpportunityCommand
            {
                JobApplicationId = jobOpportunityId,
                Description = model.Description,
                DescriptionFormat = model.Format,
            };

            var result = await Mediator.Send(command);

            return (result.IsSuccess) ? NoContent() : HandleFailureResult(result);
        }
    }
}
