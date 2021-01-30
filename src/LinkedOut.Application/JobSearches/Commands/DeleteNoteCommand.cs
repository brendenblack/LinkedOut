using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class DeleteNoteCommand : IRequest<Result>
    {
        public int NoteId { get; set; }
    }

    public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteNoteCommand> _logger;

        public DeleteNoteHandler(IApplicationDbContext context, ILogger<DeleteNoteCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {

            var note = await _context.Notes
                .FindAsync(request.NoteId);

            if (note == null)
            {
                _logger.LogWarning("Requested note with id {NoteId} was not found", request.NoteId);
                return Result.Fail("No note found");
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
