using System;
using System.Collections.Generic;


namespace Karma.Models
{
    public class User
    {
        public User()
        {
            this.Listings = new List<String>();
            this.RequestedListings = new List<String>();
            this.Comments = new List<Comment>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<String> Listings { get; set; }

        public virtual List<String> RequestedListings { get; set; }
        
        public virtual List<Comment> Comments { get; set; }

        public string AvatarPath { get; set; }

        public string Token { get; set; }
    }
}