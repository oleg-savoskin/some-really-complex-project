using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Integration.Persistence;

namespace SomeReallyComplexProject.Integration.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseIntegrationEvents(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetService<IntegrationDbContext>().Database.Migrate();
            builder.ApplicationServices.GetService<IBusControl>().Start();
        }
    }
}
