using FluentResults;
using LinkedOut.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LinkedOut.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));

        protected ActionResult HandleFailureResult(Result failureResult)
        {
            if (failureResult.IsSuccess)
            {
                throw new ArgumentException("");
            }

            if (failureResult.HasError<NotFoundError>())
            {
                return NotFound(failureResult.Reasons);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, failureResult.Reasons);
        }
    }
}
