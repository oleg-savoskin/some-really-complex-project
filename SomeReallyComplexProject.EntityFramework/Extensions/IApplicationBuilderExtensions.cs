using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.EntityFramework.DomainEvents;

namespace SomeReallyComplexProject.EntityFramework.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseDomainEvents(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetService<DomainEventsDbContext>().Database.Migrate();
        }
    }
}
