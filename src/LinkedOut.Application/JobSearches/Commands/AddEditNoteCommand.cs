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
    public class AddEditNoteCommand : IRequest<Result<int>>
    {
        public int JobApplicationId { get; set; }

        public int? NoteId { get; set; }

        public string Contents { get; set; }
    }

    public class AddNoteHandler : IRequestHandler<AddEditNoteCommand, Result<int>>
    {
        private readonly ILogger<AddNoteHandler> _logger;
        private readonly IApplicationDbContext _context;

        public AddNoteHandler(ILogger<AddNoteHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result<int>> Handle(AddEditNoteCommand request, CancellationToken cancellationToken)
        {
            if (request.NoteId.HasValue)
            {
                var note = await _context.Notes.FindAsync(request.NoteId.Value);
                note.Contents = request.Contents;
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Ok(note.Id);
            }
            else
            {
                var application = await _context.JobApplications
                    .Where(a => a.Id == request.JobApplicationId)
                    .Include(a => a.Notes)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(cancellationToken);

                var noteResult = application.AddNote(request.Contents);

                if (noteResult.IsSuccess)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger.LogDebug("Created note on application {JobApplicationId} with id {NoteId}", application.Id, noteResult.Value.Id);

                    return Result.Ok(noteResult.Value.Id);
                }

            }

            return Result.Fail("");
        }
    }
}
