using Airlines25554.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class AirportViewModel 
    {
        public int CityId { get; set; }


        public int AirportId { get; set; }


    
        [Display(Name = "Airport")]
        public string Name { get; set; }

        [Required]
        public string IATA { get; set; }


        public string CityName { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{CityName} ({IATA})";
    }
}
