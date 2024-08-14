using CleanArchitecture.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // mediatr configuration
            services.AddMediatR(configuration =>
            {
                // assembly config for mediatr
                configuration.RegisterServicesFromAssemblies(assembly);

                // configure pipeline
                configuration.AddOpenBehavior(typeof(RequestResponseLogginBehavior<,>));
            });

            // fluent validation configuration
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
