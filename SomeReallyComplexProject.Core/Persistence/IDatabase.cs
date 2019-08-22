using System;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Persistence
{
    public interface IDatabase : IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task SaveChangesAsync(bool publishEvents, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(Guid correlationId, int iteration, bool publishEvents = true, CancellationToken cancellationToken = default);
    }
}
