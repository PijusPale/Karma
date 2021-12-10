using System;

namespace Karma.Models
{
    public class ConversationDto
    {
        public string ListingName { get; set; }
        public string LastSender { get; set; }
        public int UserOneId { get; set; }
        public int UserTwoId { get; set; }
        public string UserOneName { get; set; }
        public string UserTwoName { get; set; }
        public string LastMessage { get; set; }
        public DateTime DateSent { get; set; }
        public string ListingImg { get; set; }
        public string GroupId { get; set; }
        public int ListingId { get; set; }
	}
}