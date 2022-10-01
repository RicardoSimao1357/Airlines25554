using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class FlightViewModel : Flight
    {

        public List<Flight> Flights { get; set; }

        public int StateId { get; set; }

        //SelectListItem é a combobox (Vai renderizar)
        public IEnumerable<SelectListItem> States { get; set; }

    }
}
