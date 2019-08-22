using Microsoft.Extensions.Logging;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Events;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceTwo.Application.IntegrationEventHandlers
{
    public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegrationEventHandler> logger;

        public UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(UserCreatedIntegrationEvent integrationEvent)
        {
            logger.LogInformation($"{nameof(UserCreatedIntegrationEvent)} event received, ID: {integrationEvent.EventId}");
            return Task.CompletedTask;
        }
    }
}
