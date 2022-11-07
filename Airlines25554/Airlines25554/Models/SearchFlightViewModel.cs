using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Airlines25554.Data.Entities;

namespace Airlines25554.Models
{
    public class SearchFlightViewModel
    {

        public string FromId { get; set; }

        public string ToId { get; set; }

        public int ClassId { get; set; }

        public string Class { get; set; }

        public List<Flight> Flights { get; set; }

        [Required(ErrorMessage = "Choose a flight")]
        public int flightId { get; set; }

        public IEnumerable<SelectListItem> Classes { get; set; }

      

        public IEnumerable<SelectListItem> Airports { get; set; }

        [Display(Name = "Departure")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Departure { get; set; }


    }
}
