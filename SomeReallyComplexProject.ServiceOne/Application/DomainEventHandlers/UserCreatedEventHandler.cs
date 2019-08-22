using SomeReallyComplexProject.Core.Domain.Events;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Events;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate.Events;
using System;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceOne.Application.DomainEventHandlers
{
    class UserCreatedEventHandler : DomainEventHandler<UserCreatedEvent>
    {
        private readonly IServiceOneDatabase serviceOneDatabase;
        private readonly IIntegrationService integrationService;

        public UserCreatedEventHandler(IServiceOneDatabase serviceOneDatabase, IIntegrationService integrationService)
        {
            this.serviceOneDatabase = serviceOneDatabase;
            this.integrationService = integrationService;
        }

        public async override Task Do(Guid correlationId, int iteration, UserCreatedEvent domainEvent)
        {
            if (iteration <= 3)
            {
                var user = new User($"{domainEvent.Name}+{iteration}");
                serviceOneDatabase.UserRepository.Create(user);
                await serviceOneDatabase.SaveChangesAsync(correlationId, iteration);

                await integrationService.CreateEvent(
                    new UserCreatedIntegrationEvent(domainEvent.UserId, domainEvent.Name)
                );
            }
        }
    }
}
