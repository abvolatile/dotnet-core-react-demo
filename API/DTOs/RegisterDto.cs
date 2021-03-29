using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}$", ErrorMessage = "Password must be complex")] //must have at least 1 digit, lowercase, uppercase and be btwn 6 and 12 chars
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}