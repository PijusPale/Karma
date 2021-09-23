using System;
using System.Collections.Generic;

namespace Karma.Models
{
	public class Category
	{
		public Category()
		{
			this.Listings = new List<Listing>();
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public virtual List<Listing> Listings { get; set; }

		public string ImagePath { get; set; }
	}
}