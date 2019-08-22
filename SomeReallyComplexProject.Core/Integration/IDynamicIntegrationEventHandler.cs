using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Integration
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}