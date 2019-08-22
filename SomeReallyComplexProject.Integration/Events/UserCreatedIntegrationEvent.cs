using SomeReallyComplexProject.Core.Integration;
using System;

namespace SomeReallyComplexProject.Integration.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }

        public string Username { get; }

        public UserCreatedIntegrationEvent(Guid userId, string username)
        {
            UserId = userId;
            Username = username;
        }
    }
}
