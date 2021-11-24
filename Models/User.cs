using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karma.Models
{
    public class User: Entity
    {
        public User()
        {
            this.Listings = new List<int>();
            this.RequestedListings = new List<int>();
        }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [NotMapped]
        public virtual List<int> Listings { get; set; }
        [NotMapped]
        public virtual List<int> RequestedListings { get; set; }
        [NotMapped]
        public virtual string[] Comments { get; set; }

        public string this[int index]
        {
            get => Comments[index];
            set => Comments[index] = value;
        }
        public string? AvatarPath { get; set; }
        [NotMapped]
        public string Token { get; set; }
    }
}