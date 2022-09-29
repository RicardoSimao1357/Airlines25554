using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }



        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }


        public ICollection<Airport> Airports { get; set; }


        [Display(Name = "Number of Airports")]
        public int NumberAirports => Airports == null ? 0 : Airports.Count;
    }
}
