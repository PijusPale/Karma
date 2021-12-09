using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Karma.Models
{
    public enum MessageStatus
    {
        Sent, Delivered, Read
    }

    public class Message : Entity
    {
        public Message(string content, int fromId, string groupId, DateTime dateSent, MessageStatus status)
        {
            Content = content;
            FromId = fromId;
            GroupId = groupId;
            DateSent = dateSent;
            Status = status;
        }
        [Required]
        public string Content { get; set; }
        [Required]
        [ForeignKey("UserForeignKey")]
        public int FromId { get; set; }
        [JsonIgnore]
        public User FromUser { get; set; }
        [Required]
        [ForeignKey("ConversationForeignKey")]
        public string GroupId { get; set; }
        [JsonIgnore]
        public Conversation Conversation { get; set; }
        [Required]
        public DateTime DateSent { get; set; }
        [Required]
        public MessageStatus Status { get; set; }
    }

}