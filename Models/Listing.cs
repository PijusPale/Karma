using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
	public struct Location {
        public Location(string country, string district, string city, int radiusKM)
        {
            Country = country;
            District = district;
            City = city;
            RadiusKM = radiusKM;
        }

        public string Country { get; set; }
		public string District { get; set; }
		public string City { get; set; }
		public int RadiusKM { get; set; }
	}
	public class Listing
	{
        public string? Id { get; set; }
		public string OwnerId { get; set; }

		[Required]
		[StringLength(20)]
		public string Name { get; set; }

		[StringLength(200)]
		public string Description { get; set; }

		[Required]
		[Range(1, 100)]
		public int Quantity { get; set; }
		
		[Required]
		public Location Location { get; set; }

		[Required]
		public string Category { get; set; }

		[Display(Name = "Date Published")]
		public DateTime DatePublished { get; set; }

		public string? ImagePath { get; set; }
	}	
}