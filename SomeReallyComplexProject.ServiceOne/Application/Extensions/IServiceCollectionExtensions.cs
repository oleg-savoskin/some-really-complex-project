using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SomeReallyComplexProject.ServiceOne.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);
        }
    }
}
