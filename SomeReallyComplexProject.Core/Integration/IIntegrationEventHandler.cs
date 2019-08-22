using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Integration
{
    public interface IIntegrationEventHandler<in T> where T : IntegrationEvent
    {
        Task Handle(T integrationEvent);
    }
}