using FluentResults;
using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class AddEmployerContactCommand : IRequest<Result<int>>
    {
        public int JobApplicationId { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public string Author { get; set; }
    }

    public class AddEditEmployerContactHandler : IRequestHandler<AddEmployerContactCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public AddEditEmployerContactHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(AddEmployerContactCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications
                .Where(a => a.Id == request.JobApplicationId)
                .FirstOrDefaultAsync(cancellationToken);

            var author = (string.IsNullOrWhiteSpace(request.Author)) ? application.OrganizationName : request.Author;

            var noteResult = application.RecordEmployerContact(request.Message, author, "", request.Timestamp);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(noteResult.Value.Id);
        }
    }
}
