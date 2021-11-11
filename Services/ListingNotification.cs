using System;
using System.Text.Json;
using Karma.Models;
using System.Text.Json.Serialization;
using System.IO;

namespace Karma.Services
{
    public interface IListingNotification
    {
        void Start();
        event Notify saveNotification;
    }

    public class ListingNotification : Notification, IListingNotification
    {
        protected string _filePath;
        public ListingNotification(string filePath){
            _filePath = filePath;
        }
        public void Start(){
            
            var notif = new NotificationModel{
                Text = "Your item has been requested",
                DatePublished = DateTime.UtcNow
            };
            
            string jsonString = JsonSerializer.Serialize(notif);
            File.WriteAllText(_filePath, jsonString);
            onSaveNotification();
        }
    }

}