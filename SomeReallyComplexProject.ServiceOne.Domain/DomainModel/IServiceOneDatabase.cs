using SomeReallyComplexProject.Core.Persistence;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel
{
    public interface IServiceOneDatabase : IDatabase
    {
        IUserRepository UserRepository { get; }
    }
}
