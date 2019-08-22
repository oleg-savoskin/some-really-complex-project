using SomeReallyComplexProject.Core.Domain.Events;
using System;

namespace SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate.Events
{
    public class UserJoinedGroupEvent : DomainEvent
    {
        public Guid UserId { get; }

        public Guid GroupId { get; }

        public string GroupName { get; }

        public UserJoinedGroupEvent(Guid userId, Guid groupId, string groupName)
        {
            UserId = userId;
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}
