using AutoMapper;
using FluentValidation;
using LinkedOut.Application.Common.Behaviours;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace LinkedOut.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLinkedOut(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Preprocessor logging is disabled in favour of structured logging context
            //services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ScopedLoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
