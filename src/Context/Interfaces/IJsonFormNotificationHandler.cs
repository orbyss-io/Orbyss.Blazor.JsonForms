using Orbyss.Components.JsonForms.Context.Notifications;

namespace Orbyss.Components.JsonForms.Context.Interfaces
{
    public interface IJsonFormNotificationHandler : IJsonFormNotification
    {
        void Notify(JsonFormNotificationType type);
    }

    public interface IJsonFormNotification
    {
        IDisposable Subscribe(JsonFormNotificationType type, Action callback);        
    }
}
