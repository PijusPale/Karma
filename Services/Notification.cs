using System;

namespace Karma.Services
{
    public interface INotification
    {
        EventHandler<NotificationEventArgs> getNotificationEvent();
        void onNotification(NotificationEventArgs args);
        void subscribeEvent(EventHandler<NotificationEventArgs> notificationHandler);
        event EventHandler<NotificationEventArgs> notification; 
    }

    public class Notification : INotification
    {
        public event EventHandler<NotificationEventArgs> notification;

        public EventHandler<NotificationEventArgs> getNotificationEvent()
        {
            return notification;
        }

        public void onNotification(NotificationEventArgs args){
            if (notification != null) notification(this, args);
        }

        public void subscribeEvent(EventHandler<NotificationEventArgs> notificationHandler){
            notification += notificationHandler;
        }
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Text;

        public NotificationEventArgs(string text)
        {
            Text = text;
        }
    }
}