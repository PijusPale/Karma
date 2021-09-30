using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
	public class Listing
	{
		public string Id { get; set; }

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

		public string Category { get; set; }

		[Display(Name = "Date Published")]
		public DateTime DatePublished { get; set; }

		public string ImagePath { get; set; }
	}	
}