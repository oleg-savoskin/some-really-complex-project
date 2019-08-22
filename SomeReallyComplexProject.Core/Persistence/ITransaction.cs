using System;

namespace SomeReallyComplexProject.Core.Persistence
{
    public interface ITransaction : IDisposable
    {
        void Rollback();

        void Commit();
    }
}
