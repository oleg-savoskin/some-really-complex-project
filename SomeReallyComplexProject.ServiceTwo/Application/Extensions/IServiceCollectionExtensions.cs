using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SomeReallyComplexProject.ServiceTwo.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);
        }
    }
}
