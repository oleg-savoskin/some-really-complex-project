using MassTransit;
using Microsoft.Extensions.Configuration;
using SomeReallyComplexProject.Common.Extensions;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Persistence;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Integration
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IBus eventBus;
        private readonly IEventsRepository eventsRepository;
        private readonly IConfiguration configuration;

        public IntegrationService(
            IBus eventBus,
            IConfiguration configuration,
            IEventsRepository eventsRepository)
        {
            this.eventBus = eventBus;
            this.configuration = configuration;
            this.eventsRepository = eventsRepository;
        }

        public Task CreateEvent(IntegrationEvent integrationEvent)
        {
            var sender = configuration.GetServiceUID();
            var record = new IntegrationEventRecord(sender, integrationEvent);
            return eventsRepository.Track(record);
        }

        public async Task PublishPendingEvents()
        {
            var pendingEvents = await eventsRepository.GetPending();

            foreach (var pendingEvent in pendingEvents)
            {
                pendingEvent.MarkAsPublished();
                await eventBus.Publish(pendingEvent);
                await eventsRepository.Track(pendingEvent);
            }
        }
    }
}
