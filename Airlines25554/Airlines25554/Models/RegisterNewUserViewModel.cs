using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class RegisterNewUserViewModel : Customer
    {

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]    
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
    }
}
