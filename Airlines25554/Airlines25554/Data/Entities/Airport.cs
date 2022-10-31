using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Airlines25554.Data.Entities
{
    public class Airport : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Airport")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

        [Required]
        public string IATA { get; set; }

        [Required]
        public string CityName { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{CityName} ({IATA})";
    }
}
