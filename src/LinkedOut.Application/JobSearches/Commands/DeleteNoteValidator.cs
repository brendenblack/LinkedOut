using FluentValidation;
using LinkedOut.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class DeleteNoteValidator : AbstractValidator<DeleteNoteCommand>
    {
        public DeleteNoteValidator(ICurrentUserService currentUserService, IApplicationDbContext context)
        {
            var ownerId = context.Notes
                .Select(n => n.Application.ParentSearch.OwnerId)
                .FirstOrDefault();

            RuleFor(c => c.NoteId)
                .Must(id => context.Notes.Any(n => n.Id == id))
                .WithMessage("Note doesn't exist");

            RuleFor(c => c.NoteId)
                .Must(id => currentUserService.UserId == ownerId)
                .WithMessage("You cannot delete a note that you were not the author of.");
        }
    }
}
