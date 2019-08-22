using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Common.Extensions;
using SomeReallyComplexProject.EntityFramework.Extensions;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel;
using SomeReallyComplexProject.ServiceOne.Domain.Infrastructure;

namespace SomeReallyComplexProject.ServiceOne.Domain.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddServiceOneDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSqlConnectionString();
            services.AddTransient<IServiceOneDatabase, ServiceOneDatabase>();
            services.AddDomainEvents(connectionString, typeof(ServiceOneContext).Assembly);
            services.AddDbContext<ServiceOneContext>(o => o.UseSqlServer(connectionString),
                ServiceLifetime.Transient, ServiceLifetime.Transient);
        }
    }
}
