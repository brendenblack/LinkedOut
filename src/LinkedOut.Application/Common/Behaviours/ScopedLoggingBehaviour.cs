using LinkedOut.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.Application.Common.Behaviours
{
    /// <summary>
    /// Creates a scoped logging context for the pipeline request, enriching each message with a request ID and the requestor ID. 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ScopedLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ScopedLoggingBehaviour<TRequest, TResponse>> _logger;

        public ScopedLoggingBehaviour(ICurrentUserService currentUser, ILogger<ScopedLoggingBehaviour<TRequest, TResponse>> logger)
        {
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var scopeDictionary = new Dictionary<string, object>
            {
                ["RequestorId"] = _currentUser.UserId,
                ["RequestId"] = Guid.NewGuid().ToString()
            };

            using (_logger.BeginScope(scopeDictionary))
            {
                _logger.LogDebug("{Name}: {@Request}", 
                    typeof(TRequest).Name,
                    _currentUser.UserId,
                    request);

                return await next();
            }
        }
    }
}
