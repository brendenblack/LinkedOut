using LinkedOut.Application.JobSearches.Commands;
using LinkedOut.Application.JobSearches.Queries.GetJobApplication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        [HttpPost("{jobApplicationId:int}/notes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddNote([FromRoute] int jobApplicationId, [FromBody] string note)
        {
            var command = new AddEditNoteCommand
            {
                JobApplicationId = jobApplicationId,
                Contents = note
            };

            var result = await Mediator.Send(command);

            return (result.IsSuccess)
                ? CreatedAtAction(nameof(GetNote), new { jobApplicationId = jobApplicationId, noteId = result.Value }, result.Value)
                : HandleFailureResult(result);
        }

        [HttpGet("{jobApplicationId:int}/notes/{noteId:int}")]
        public async Task<ActionResult> GetNote([FromRoute] int jobApplicationId, [FromRoute] int noteId)
        {
            return NoContent();
        }

        [HttpDelete("{jobApplicationId:int}/notes/{noteId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteNote([FromRoute] int jobApplicationId, [FromRoute] int noteId)
        {
            var command = new DeleteNoteCommand
            {
                NoteId = noteId,
            };

            var result = await Mediator.Send(command);

            return (result.IsSuccess) ? NoContent() : HandleFailureResult(result);
        }
    }
}
