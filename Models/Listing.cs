using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Karma.Models
{
    public enum Condition
    {
        New, Used, Broken
    }
    public struct Location
    {
        public Location(string country, string district, string city, int radiusKM)
        {
            Country = country;
            District = district;
            City = city;
            this.radiusKM = radiusKM < 0 ? 0 : radiusKM;
        }

        public string Country { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        private int radiusKM;
        public int RadiusKM
        {
            get => radiusKM;
            set => radiusKM = value < 0 ? 0 : value;
        }
    }
    public class Listing : Entity, IComparable
    {
        public Listing()
        {
            this.RequestedUserIDs = new List<int>();
        }

        public int CompareTo(object obj)
        {
			    if (obj == null) return 1;
			       Listing otherListing = obj as Listing;
			    if (otherListing != null)
				     return this.DatePublished.CompareTo(otherListing.DatePublished);
			    else
				     throw new ArgumentException("Object is not a Listing");
        }
        public int OwnerId { get; set; }

        public bool isReserved { get; set; }

        public string? recipientId { get; set; }

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

        //[Required]
        [NotMapped]
        public Location? Location { get; set; }

        public string LocationJson {
            get {
                return JsonSerializer.Serialize(Location);
            }
            set {
                if (value == null)
                    Location = null;
                else 
                Location = JsonSerializer.Deserialize<Location>(value);
            }
        }

        [Required]
        public string Category { get; set; }

        [Display(Name = "Date Published")]
        public DateTime DatePublished { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public Condition Condition { get; set; }

        [NotMapped]
        public virtual List<int> RequestedUserIDs { get; set; }
    }
}