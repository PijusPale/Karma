using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Karma.Helpers;
using Karma.Models;
using Karma.Models.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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