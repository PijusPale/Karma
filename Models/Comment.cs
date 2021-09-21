using System;
using System.Collections.Generic;

namespace Karma.Models
{
	public class Comment
	{
		public string Id { get; set; }

		public string Content { get; set; }

		public string ListingId { get; set; }

		public virtual Listing Listing { get; set; }

		public string UserId { get; set; }

		public virtual User User { get; set; }

		public DateTime DatePublished { get; set; }
	}
}