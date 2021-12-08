using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Karma.Models
{
	public class Comment: Entity
	{
		public string Content { get; set; }

		public string ListingId { get; set; }

		[JsonIgnore]
		public virtual Listing Listing { get; set; }

		public string UserId { get; set; }

		[JsonIgnore]
		public virtual User User { get; set; }

		public DateTime DatePublished { get; set; }
	}
}