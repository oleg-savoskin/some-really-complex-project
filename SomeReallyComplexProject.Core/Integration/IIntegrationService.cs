using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Integration
{
    public interface IIntegrationService
    {
        Task CreateEvent(IntegrationEvent integrationEvent);

        Task PublishPendingEvents();
    }
}
