using SomeReallyComplexProject.Core.Integration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeReallyComplexProject.Integration
{
    public class SubscriptionsManager : ISubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> handlers;
        private readonly List<Type> eventTypes;

        public bool IsEmpty => !handlers.Keys.Any();

        public IEnumerable<Type> EventHandlers
        {
            get => handlers.Values.SelectMany(o => o.Select(x => x.HandlerType)).Distinct();
        }

        public event EventHandler<string> OnEventRemoved;

        public SubscriptionsManager()
        {
            handlers = new Dictionary<string, List<SubscriptionInfo>>();
            eventTypes = new List<Type>();
        }

        public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            DoAddSubscription(typeof(TH), GetEventKey<T>(), isDynamic: false);
            eventTypes.Add(typeof(T));
        }

        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            DoRemoveHandler(GetEventKey<T>(), handlerToRemove);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            return GetHandlersForEvent(GetEventKey<T>());
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            return HasSubscriptionsForEvent(GetEventKey<T>());
        }

        public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => handlers[eventName];

        public Type GetEventTypeByName(string eventName) => eventTypes.SingleOrDefault(t => t.Name == eventName);

        public void Clear() => handlers.Clear();

        public string GetEventKey<T>() => typeof(T).Name;

        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            return DoFindSubscriptionToRemove(GetEventKey<T>(), typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
                return null;

            return handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                handlers[eventName].Remove(subsToRemove);

                if (!handlers[eventName].Any())
                {
                    handlers.Remove(eventName);

                    var eventType = eventTypes.SingleOrDefault(e => e.Name == eventName);

                    if (eventType != null)
                        eventTypes.Remove(eventType);

                    OnEventRemoved?.Invoke(this, eventName);
                }
            }
        }
    }
}
