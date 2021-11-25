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
        public Message(int id, string content, int fromId, string groupId, DateTime dateSent, MessageStatus status)
        {
            Id = id;
            Content = content;
            FromId = fromId;
            GroupId = groupId;
            DateSent = dateSent;
            Status = status;
        }
        [Required]
        public string Content { get; set; }
        [Required]
        public int FromId { get; set; }
        [Required]
        public string GroupId { get; set; }
        [Required]
        public DateTime DateSent { get; set; }
        [Required]
        public MessageStatus Status { get; set; }
    }

}