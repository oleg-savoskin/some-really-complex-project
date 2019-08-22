using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.EntityFramework.Extensions;
using SomeReallyComplexProject.ServiceOne.Domain.Infrastructure;

namespace SomeReallyComplexProject.ServiceOne.Domain.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseServiceOneDomainServices(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetService<ServiceOneContext>().Database.Migrate();
            builder.UseDomainEvents();
        }
    }
}
