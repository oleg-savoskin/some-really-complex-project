using System;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Persistence
{
    public interface IEventualConsistencyService
    {
        Task EnsureLogicalTransactionCompletedAsync(Guid correlationId);
    }
}
