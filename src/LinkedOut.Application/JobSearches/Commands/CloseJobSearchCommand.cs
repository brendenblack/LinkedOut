using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.JobSearches.Commands
{
    public class CloseJobSearchCommand : IRequest<Result>
    {
    }

    public class CloseJobSearchHandler : IRequestHandler<CloseJobSearchCommand, Result>
    {
        public Task<Result> Handle(CloseJobSearchCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
