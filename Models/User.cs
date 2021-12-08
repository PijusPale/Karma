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
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Listing> Listings { get; set; }
        [JsonIgnore]
        public virtual ICollection<Listing> RequestedListings { get; set; }
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

        public int GardenId { get; set; }

        [JsonIgnore]
        public Garden Garden { get; set; }
    }
}