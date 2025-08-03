using Orbyss.Components.JsonForms.Context.Interfaces;

namespace Orbyss.Components.JsonForms.Context.Notifications
{
    public sealed class JsonFormNotificationHandler : IJsonFormNotificationHandler
    {
        private readonly Dictionary<Guid, NotificationCallback> _subscribers = [];

        public IDisposable Subscribe(JsonFormNotificationType type, Action callback)
        {
            var token = new SubscriptionToken(Guid.NewGuid(), (id) => _subscribers.Remove(id));
            _subscribers.Add(token.Id, new(type, callback));
            return token;
        }

        public void Notify(JsonFormNotificationType type)
        {
            foreach (var subscriber in _subscribers.Values.Where(x => x.Type == type))
                subscriber.Callback();
        }

        private readonly struct NotificationCallback(JsonFormNotificationType type, Action callback)
        {
            public JsonFormNotificationType Type { get; } = type;
            public Action Callback { get; } = callback;
        }

        private sealed record SubscriptionToken(Guid Id, Action<Guid> Unsubscribe) : IDisposable
        {
            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    Unsubscribe(Id);
                    _disposed = true;
                }
            }
        }
    }
}