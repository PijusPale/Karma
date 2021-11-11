using System.Collections.Generic;
using System;

namespace Karma.Services
{
    public delegate void Notify();

    public class Notification
    {
        public event Notify saveNotification;

        protected virtual void onSaveNotification(){
            saveNotification?.Invoke();
        }
        
    }

}