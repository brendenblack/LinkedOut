using FluentResults;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResultBase, new()
        // TODO: i think this will mean that this behaviour either won't work with a non-Result-based 
        //       response, or that it will throw an exception
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    if (typeof(TResponse) == typeof(Result))
                    {
                        Result result = null;
                        foreach (var failure in failures)
                        {
                            // TODO there has to be a less ugly way to do this
                            if (result == null)
                            {
                                result = Result.Fail(failure.ErrorMessage);
                            }
                            else
                            {
                                result.WithError(failure.ErrorMessage);
                            }
                        }

                        return result as TResponse;
                    }

                    throw new ValidationException(failures);
                }
                    
            }
            return await next();
        }
    }
}
