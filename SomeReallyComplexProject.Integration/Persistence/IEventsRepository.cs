using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Integration.Persistence
{
    public interface IEventsRepository
    {
        Task Track(IntegrationEventRecord integrationEvent);

        Task<IEnumerable<IntegrationEventRecord>> GetPending();

        Task<bool> IsHandled(Guid eventID);
    }
}
