﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
	public class Listing
	{
		public Listing()
		{
			this.RequestedUserIDs = new List<String>();
		}

        public string? Id { get; set; }
		
		public string OwnerId { get; set; }

		[Required]
        [RegularExpression(@"^[a-zA-Z0-9! ]+$")]
		[StringLength(20)]
		public string Name { get; set; }

		[StringLength(200)]
		[RegularExpression(@"^[a-zA-Z0-9!+, ]+$")]
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

		public virtual List<String> RequestedUserIDs { get; set; }
	}	
}