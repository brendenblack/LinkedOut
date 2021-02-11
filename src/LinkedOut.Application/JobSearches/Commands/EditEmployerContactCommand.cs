using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using LinkedOut.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class EditEmployerContactCommand : IRequest<Result<int>>
    {
        public int EmployerContactId { get; set; }

        public string Author { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class EditEmployerContactHandler : IRequestHandler<EditEmployerContactCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public EditEmployerContactHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(EditEmployerContactCommand request, CancellationToken cancellationToken)
        {
            var employerContact = await _context.Notes
                .FindAsync(request.EmployerContactId);

            if (employerContact == null)
            {
                return Result.Fail(new NotFoundError(nameof(Note), request.EmployerContactId));
            }

            employerContact.Author = request.Author;
            employerContact.Timestamp = request.Timestamp;
            employerContact.Contents = request.Message;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(employerContact.Id);
        }
    }
}
