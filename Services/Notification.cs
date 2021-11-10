using System;

namespace Karma.Services
{
    public interface INotification
    {
        EventHandler<NofiticationEventArgs> getNotification();
    }

    public class Notification : INotification
    {
        private event EventHandler<NofiticationEventArgs> notification;

        public EventHandler<NofiticationEventArgs> getNotification()
        {
            return notification;
        }
    }

    public class NofiticationEventArgs : EventArgs
    {
        public string Text;

        public NofiticationEventArgs(string text)
        {
            Text = text;
        }
    }
}