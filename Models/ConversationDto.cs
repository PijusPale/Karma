using System;
using System.Collections.Generic;

namespace Karma.Models
{
	public class ConversationDto
    {
        public string ListingTitle { get; set; }
        public string LastSender { get; set; }
        public string LastMessage { get; set; }
        public string ListingImg { get; set; }
        public string GroupId { get; set; }
	}
}