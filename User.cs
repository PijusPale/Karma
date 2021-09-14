using System;
using System.Collections.Generic;


namespace Karma
{
	public class User
	{
		public User(string imagePath)
		{
			this.Listings = new List<Listing>();
			this.Comments = new List<Comment>();
		}

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public virtual List<Listing> Listings { get; set; }

		public virtual List<Comment> Comments { get; set; }

		public string AvatarPath { get; set; }
	}
}