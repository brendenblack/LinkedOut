using FluentValidation;
using LinkedOut.Application.Common.Interfaces;
using System.Linq;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class DeleteJobSearchCommandValidator : AbstractValidator<DeleteJobSearchCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteJobSearchCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(c => c.JobSearchId)
                .Must(id => context.JobSearches.Any(s => s.Id == id));

            RuleFor(c => c.JobSearchId)
                .Must(BeOwner)
                .WithMessage("Only the owner of a job search may delete it.");

        }

        public bool BeOwner(int jobSearchId)
        {
            var search = _context.JobSearches.Find(jobSearchId);
            if (search == null)
            {
                return false;
            }

            return search.OwnerId == _currentUserService.UserId;
        }
    }
}
