using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Karma.Models
{
    public enum MessageStatus
    {
        Sent, Delivered, Read
    }

    public class Message : Entity
    {
        public Message(string content, string fromId, string groupId, DateTime dateSent, MessageStatus status)
        {
            Content = content;
            FromId = fromId;
            GroupId = groupId;
            dateSent = DateSent;
            Status = status;
        }

        public string Content { get; set; }

        public string FromId { get; set; }

        public string GroupId { get; set; }

        public DateTime DateSent { get; set; }

        public MessageStatus Status { get; set; }
    }

}