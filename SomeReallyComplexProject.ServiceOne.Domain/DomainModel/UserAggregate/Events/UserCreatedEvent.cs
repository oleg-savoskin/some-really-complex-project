using SomeReallyComplexProject.Core.Domain.Events;
using System;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public Guid UserId { get; }

        public string Name { get; }

        public UserCreatedEvent(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
