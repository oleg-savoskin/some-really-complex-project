using Newtonsoft.Json;
using SomeReallyComplexProject.Core.Integration;
using System;

namespace SomeReallyComplexProject.Integration.Persistence
{
    public class IntegrationEventRecord
    {
        public Guid EventId { get; private set; }

        public string Sender { get; private set; }

        public string EventName { get; private set; }

        public string EventData { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime? DateHandled { get; private set; }

        public DateTime? DatePublished { get; private set; }

        public void MarkAsPublished() => DatePublished = DateTime.UtcNow;

        public void MarkAsHandled() => DateHandled = DateTime.UtcNow;

        public IntegrationEventRecord(string sender, IntegrationEvent @event)
        {
            Sender = sender;
            EventId = Guid.NewGuid();
            EventName = @event.GetType().Name;
            EventData = JsonConvert.SerializeObject(@event);
            DateCreated = DateTime.UtcNow;
        }

        private IntegrationEventRecord()
        {
        }
    }
}
