using System.ComponentModel.DataAnnotations;

namespace Karma.Models.Authentication
{
    public class UserCredentials
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}