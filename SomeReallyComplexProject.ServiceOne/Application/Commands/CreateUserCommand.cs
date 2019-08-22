using MediatR;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;

namespace SomeReallyComplexProject.ServiceOne.Application.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public string Username { get; }

        public CreateUserCommand(string username)
        {
            Username = username;
        }
    }
}
