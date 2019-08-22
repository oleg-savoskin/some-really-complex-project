using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;
using System;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel
{
    public interface IUserRepository
    {
        Task<User> Get(Guid userID);

        void Create(User user);

        void Update(User user);
    }
}
