using System;
using System.Collections.Generic;


namespace Karma.Models
{
    public class User: Entity
    {
        public User()
        {
            this.Listings = new List<String>();
            this.RequestedListings = new List<int>();
        }
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<String> Listings { get; set; }

        public virtual List<int> RequestedListings { get; set; }
        
        public virtual string[] Comments { get; set; }

        public string this[int index]
        {
            get => Comments[index];
            set => Comments[index] = value;
        }

        public string AvatarPath { get; set; }

        public string Token { get; set; }
    }
}