using MediatR;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.ServiceOne.Application.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IServiceOneDatabase serviceOneDatabase;

        public CreateUserCommandHandler(IServiceOneDatabase serviceOneDatabase)
        {
            this.serviceOneDatabase = serviceOneDatabase;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username);
            serviceOneDatabase.UserRepository.Create(user);
            await serviceOneDatabase.SaveChangesAsync();
            return user;
        }
    }
}
