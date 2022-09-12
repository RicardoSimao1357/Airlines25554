using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class LoginViewModel
    {
        [Required]
        
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
