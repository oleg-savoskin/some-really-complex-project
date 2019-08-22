using Microsoft.EntityFrameworkCore;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;
using System;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceOne.Domain.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ServiceOneContext context;

        public UserRepository(ServiceOneContext context)
        {
            this.context = context;
        }

        public void Create(User user)
        {
            context.Users.Add(user);
        }

        public async Task<User> Get(Guid userID)
        {
            var user = await context.Users.FindAsync(userID);

            if (user != null)
                await context.Entry(user).Collection("Groups").LoadAsync();

            return user;
        }

        public void Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
