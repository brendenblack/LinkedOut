using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Application.JobSearches.Commands;
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

        [HttpGet("{jobSearchId:int}")]
        public async Task<ActionResult<JobSearchDto>> GetJobSearch([FromRoute] int jobSearchId)
        {
            var query = new GetJobSearchDetailsQuery
            {
                JobSearchId = jobSearchId
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
                return Ok(result.Value);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Reasons);
            }
        }
    }
}
