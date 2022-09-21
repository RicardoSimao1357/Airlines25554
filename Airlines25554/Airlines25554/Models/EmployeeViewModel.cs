using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class EmployeeViewModel : Employee 
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
