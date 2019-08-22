using Microsoft.EntityFrameworkCore.Storage;
using SomeReallyComplexProject.Core.Persistence;
using System.Data.Common;

namespace SomeReallyComplexProject.EntityFramework
{
    public class EFTransaction : ITransaction
    {
        readonly IDbContextTransaction transaction = null;

        public DbTransaction Instance => transaction.GetDbTransaction();

        public EFTransaction(IDbContextTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}
