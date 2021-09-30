using System;
using System.Collections.Generic;

namespace Karma.Models
{ 
	public class Listing
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public string Location { get; set; }

		public virtual Category Category { get; set; }

		public DateTime DatePublished { get; set; }

		public string ImagePath { get; set; }
	}	
}