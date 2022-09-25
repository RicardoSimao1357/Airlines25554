using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class EmployeeViewModel : Employee 
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Username")]
        public string Username { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
    }
}
