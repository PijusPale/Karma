using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
	public class Listing : IComparable
	{
        public int CompareTo(object obj)
        {
			if (obj == null) return 1;
			Listing otherListing = obj as Listing;
			if (otherListing != null)
				return this.DatePublished.CompareTo(otherListing.DatePublished);
			else
				throw new ArgumentException("Object is not a Listing");
        }
		
		public string? Id { get; set; }

		[Required]
		[StringLength(20)]
		public string Name { get; set; }

		[StringLength(200)]
		public string Description { get; set; }

		[Required]
		[Range(1, 100)]
		public int Quantity { get; set; }
		
		[Required]
		public string Location { get; set; }

		[Required]
		public string Category { get; set; }

		[Display(Name = "Date Published")]
		public DateTime DatePublished { get; set; }

		public string? ImagePath { get; set; }
	}	
}