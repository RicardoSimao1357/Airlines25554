using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class CustomerViewModel : Customer
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
