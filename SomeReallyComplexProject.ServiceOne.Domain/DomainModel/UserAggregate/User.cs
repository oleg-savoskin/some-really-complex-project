using SomeReallyComplexProject.Core.Domain;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate
{
    public class User : Entity<Guid>
    {
        private IList<UserGroup> Groups { get; }

        public string Name { get; private set; }

        public DateTime DateCreated { get; private set; }

        public User(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Parameter cannot be empty", nameof(name));

            Name = name;
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;

            AddDomainEvent(new UserCreatedEvent(Id, name));
        }

        public void JoinGroup(string groupName)
        {
            if (Groups.Any(e => e.GroupName == groupName))
                throw new ArgumentException("Name must be unique", nameof(groupName));

            var group = new UserGroup(groupName);
            Groups.Add(group);

            AddDomainEvent(new UserJoinedGroupEvent(Id, group.Id, groupName));
        }

        private User()
        {
            Groups = new List<UserGroup>();
        }
    }
}
