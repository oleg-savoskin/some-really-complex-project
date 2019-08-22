using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Common.Extensions;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Behaviors;
using SomeReallyComplexProject.Integration.Persistence;
using System;

namespace SomeReallyComplexProject.Integration.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddIntegrationEvents(this IServiceCollection services,
            IConfiguration configuration, Action<ISubscriptionsManager> configureEventHandlers = null)
        {
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IntegrationBehavior<,>));
            services.AddDbContext<IntegrationDbContext>(o => o.UseSqlServer(configuration.GetSqlConnectionString()),
                                                             ServiceLifetime.Transient, ServiceLifetime.Transient);

            var hasSubscriptions = ConfigureSubscriptions(services, configureEventHandlers);
            ConfigureMasstransit(services, configuration, hasSubscriptions);
        }

        private static bool ConfigureSubscriptions(IServiceCollection services, Action<ISubscriptionsManager> configureEventHandlers)
        {
            var subscriptionsManager = new SubscriptionsManager();
            configureEventHandlers?.Invoke(subscriptionsManager);

            foreach (var handler in subscriptionsManager.EventHandlers)
                services.AddTransient(handler);

            services.AddSingleton<ISubscriptionsManager>(subscriptionsManager);
            return !subscriptionsManager.IsEmpty;
        }

        private static void ConfigureMasstransit(IServiceCollection services, IConfiguration configuration, bool shouldListen)
        {
            services.AddMassTransit(massTransitConfig =>
            {
                if (shouldListen)
                    massTransitConfig.AddConsumer<IntegrationEventConsumer>();

                massTransitConfig.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(rabbitConfig =>
                {
                    var options = configuration.GetRabbitMqConfiguration();

                    var host = rabbitConfig.Host(options.RabbitHost, options.RabbitPort, options.RabbitVHost, hostConfig =>
                    {
                        hostConfig.Username(options.RabbitUser);
                        hostConfig.Password(options.RabbitPass);
                    });

                    if (!shouldListen)
                        return;

                    rabbitConfig.ReceiveEndpoint(host, options.RabbitQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<IntegrationEventConsumer>(provider);
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.PrefetchCount = 10;
                    });
                }));
            });
        }
    }
}
