using Dapper;
using Microsoft.EntityFrameworkCore;
using SomeReallyComplexProject.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.EntityFramework
{
    public class EFDatabase : IRelationalDatabase
    {
        private bool disposed = false;

        protected EFContext Context { get; }

        public EFDatabase(EFContext context) => Context = context;

        public IEnumerable<TResult> ExecuteQuery<TResult>(string query, object parameters = null)
        {
            return Context.Database.GetDbConnection().Query<TResult>(query, parameters);
        }

        public object ExecuteScalar(string query, object parameters = null)
        {
            return Context.Database.GetDbConnection().ExecuteScalar(query, parameters);
        }

        public int ExecuteCommand(string query, object parameters = null)
        {
            return Context.Database.GetDbConnection().Execute(query, parameters);
        }

        public Task SaveChangesAsync(Guid correlationId, int iteration, bool publishEvents = true, CancellationToken cancellationToken = default)
        {
            return Context.SaveEntitiesAsync(correlationId, iteration, publishEvents, cancellationToken);
        }

        public Task SaveChangesAsync(Guid correlationId, int iteration, ITransaction transaction, bool publishEvents = true, CancellationToken cancellationToken = default)
        {
            Context.Database.UseTransaction((transaction as EFTransaction).Instance);
            return Context.SaveEntitiesAsync(correlationId, iteration, publishEvents, cancellationToken);
        }

        public Task SaveChangesAsync(ITransaction transaction, bool publishEvents = true, CancellationToken cancellationToken = default)
        {
            Context.Database.UseTransaction((transaction as EFTransaction).Instance);
            return Context.SaveEntitiesAsync(null, null, publishEvents, cancellationToken);
        }

        public Task SaveChangesAsync(bool publishEvents, CancellationToken cancellationToken = default)
        {
            return Context.SaveEntitiesAsync(null, null, publishEvents, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveEntitiesAsync(null, null, true, cancellationToken);
        }

        public ITransaction BeginTransaction()
        {
            return new EFTransaction(Context.Database.BeginTransaction());
        }

        public void Dispose()
        {
            lock (this) Dispose(!disposed);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            Context.Dispose();
            disposed = true;
        }
    }
}
