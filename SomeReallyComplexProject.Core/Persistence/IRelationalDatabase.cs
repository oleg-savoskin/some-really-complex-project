using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Persistence
{
    public interface IRelationalDatabase : IDatabase
    {
        ITransaction BeginTransaction();

        int ExecuteCommand(string query, object parameters = null);

        object ExecuteScalar(string query, object parameters = null);

        IEnumerable<TResult> ExecuteQuery<TResult>(string query, object parameters = null);

        Task SaveChangesAsync(ITransaction transaction, bool publishEvents = true, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(Guid correlationId, int iteration, ITransaction transaction, bool publishEvents = true, CancellationToken cancellationToken = default);
    }
}
