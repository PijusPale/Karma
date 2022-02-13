using System.ComponentModel.DataAnnotations;

namespace Karma.Models.Authentication
{
    public class UserCredentials
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}