using CleanArchitecture.Repositories.Abstraction.Repos;
using CleanArchitecture.Repositories.Repos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // services registration
            services.AddTransient<IToDoItemRepository, ToDoItemRepository>();


            return services;
        }
    }
}
