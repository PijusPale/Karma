using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Karma.Models
{
    public class User: Entity
    {
        public User()
        {
            Listings = new List<Listing>();
            RequestedListings = new List<Listing>();
        }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string Password { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Listing> Listings { get; set; }
        [JsonIgnore]
        public virtual ICollection<Listing> RequestedListings { get; set; }
        [JsonIgnore]
        public virtual ICollection<Conversation> StartedConversations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Conversation> ParticipatingConversations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }
        [NotMapped]
        public virtual string[] Comments { get; set; }

        public string this[int index]
        {
            get => Comments[index];
            set => Comments[index] = value;
        }
        public string? AvatarPath { get; set; }
        public DateTime? LastActive { get; set; }
        [NotMapped]
        public string Token { get; set; }
    }
}