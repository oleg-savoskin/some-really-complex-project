using SomeReallyComplexProject.Core.Domain;
using System;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate
{
    public class UserGroup : Entity<Guid>
    {
        public Guid UserID { get; private set; }

        public string GroupName { get; private set; }

        public UserGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Parameter cannot be empty", nameof(groupName));

            Id = Guid.NewGuid();
            GroupName = groupName;
        }
    }
}
