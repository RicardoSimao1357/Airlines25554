using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
