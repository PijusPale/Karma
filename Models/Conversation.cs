using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Karma.Models
{
	public class Conversation : Entity
    {
        [Required]
        [ForeignKey("UserForeignKey")]
        public int UserOneId { get; set; }
        [JsonIgnore]
        public User UserOne { get; set; }
        [Required]
        [ForeignKey("UserForeignKey")]
        public int UserTwoId { get; set; }
        [JsonIgnore]
        public User UserTwo { get; set; }
        [Required]
        [ForeignKey("ListingForeignKey")]
        public int ListingId { get; set; }
        [JsonIgnore]
        public Listing Listing { get; set; }
        [Required]
        public string GroupId { get; set; }
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; }
	}
}