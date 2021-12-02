using System.ComponentModel.DataAnnotations;

namespace Karma.Models
{
	public class Conversation : Entity
    {
        [Required]
        public int UserOneId { get; set; }
        [Required]
        public int UserTwoId { get; set; }
        [Required]
        public int ListingId { get; set; }
        [Required]
        public string GroupId { get; set; }
	}
}