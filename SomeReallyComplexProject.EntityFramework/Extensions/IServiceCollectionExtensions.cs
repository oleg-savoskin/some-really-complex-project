using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Core.Domain.Events;
using SomeReallyComplexProject.Core.Persistence;
using SomeReallyComplexProject.EntityFramework.DomainEvents;
using System;
using System.Linq;
using System.Reflection;

namespace SomeReallyComplexProject.EntityFramework.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDomainEvents(this IServiceCollection services, string connectionString, Assembly assemblyToLoadEventsFrom)
        {
            services.AddDbContext<DomainEventsDbContext>(o => o.UseSqlServer(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);
            services.AddSingleton(serviceProvider => GetEventualConsistencyService(serviceProvider, assemblyToLoadEventsFrom));
            services.AddTransient<IDomainEventsLogService, DomainEventsLogService>();
        }

        private static IEventualConsistencyService GetEventualConsistencyService(IServiceProvider provider, Assembly assembly)
        {
            var domainEvents = assembly.GetTypes().Where(type => typeof(DomainEvent).IsAssignableFrom(type)).ToArray();
            return new EventualConsistencyService(provider.GetService<DomainEventsDbContext>(), provider.GetService<IMediator>(), domainEvents);
        }
    }
}
