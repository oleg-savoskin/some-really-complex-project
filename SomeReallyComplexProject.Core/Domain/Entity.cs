using SomeReallyComplexProject.Core.Domain.Events;
using System.Collections.Generic;

namespace SomeReallyComplexProject.Core.Domain
{
    public abstract class Entity
    {
        List<DomainEvent> domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents;

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            domainEvents = domainEvents ?? new List<DomainEvent>();
            domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            domainEvents?.Clear();
        }
    }

    public abstract class Entity<TKey> : Entity
    {
        int? requestedHashCode;

        TKey id;
        public virtual TKey Id
        {
            get => id;
            protected set => id = value;
        }

        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity<TKey> item = (Entity<TKey>)obj;

            if (item.IsTransient() || IsTransient())
                return false;

            return EqualityComparer<TKey>.Default.Equals(Id, item.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = Id.GetHashCode() ^ 31;

                return requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
    }
}
