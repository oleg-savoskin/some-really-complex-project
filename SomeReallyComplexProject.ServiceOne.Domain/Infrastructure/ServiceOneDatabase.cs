using SomeReallyComplexProject.EntityFramework;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel;
using SomeReallyComplexProject.ServiceOne.Domain.Infrastructure.Repositories;

namespace SomeReallyComplexProject.ServiceOne.Domain.Infrastructure
{
    public class ServiceOneDatabase : EFDatabase, IServiceOneDatabase
    {
        public IUserRepository UserRepository { get; }

        public ServiceOneDatabase(ServiceOneContext context) : base(context)
        {
            UserRepository = new UserRepository(context);
        }
    }
}
