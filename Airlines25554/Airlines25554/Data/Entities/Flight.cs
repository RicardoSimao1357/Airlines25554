using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        public AirPlane AirPlane { get; set; }  
        
        public string From { get; set; }

        public string To { get; set; }


        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }

        [Display(Name = "Airport")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Airport.")]
        public int AirportId { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        public int CityId { get; set; }


        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int CountryId { get; set; }


        public string Status { get; set; }  


        public IEnumerable<SelectListItem> Cities { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Airports { get; set; }

        public int busyEconomicSeats { get; set; }

        public int busyExecutiveSeats { get; set; }

        public int busyFirstClassSeats { get; set; }





    }
}
