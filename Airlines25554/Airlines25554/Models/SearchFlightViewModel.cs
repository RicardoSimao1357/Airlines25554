using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Airlines25554.Models
{
    public class SearchFlightViewModel
    {

        public string From { get; set; }

        public string To { get; set; }

        public int ClassId { get; set; }

        public string Class { get; set; }   

        public IEnumerable<SelectListItem> Classes { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Departure")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Departure { get; set; }


    }
}
