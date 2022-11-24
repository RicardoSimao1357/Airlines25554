using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class CreateFlightViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Flight Nº")]
        [Required(ErrorMessage = "The field {0} is required")]
        public string FlightNumber { get; set; }

      
        public string From { get; set; }

     
        // Destino deve ser diferente da partida
        public string To { get; set; }

        [Range(10.00, 100.00)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal EconomicTicketPrice { get; set; }

        [Range(10.00, 200.00)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ExecutiveTicketPrice { get; set; }

        [Range(10.00, 300.00)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal FirstClassTicketPrice { get; set; }



        [Display(Name = "Departure")]
        //  [Attributes.DateAfterNow()]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Departure { get; set; }



        [Display(Name = "Arrival")]
        //  [Attributes.DateAfterNow()]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        //Data de chegada deve ser depois data de partida
        //   [Attributes.DateAfterThan("Departure", "Arrival")]
        public DateTime Arrival { get; set; }



     
        [Display(Name = "Airplane")]
        public string Airplane { get; set; }

        [Range(1, 2, ErrorMessage = "Please, choose one of the states: active or canceled")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        //SelectListItem é a combobox (Vai renderizar)
        public IEnumerable<SelectListItem> Airplanes { get; set; }



        //SelectListItem é a combobox (Vai renderizar)
        public IEnumerable<SelectListItem> Airports { get; set; }



        //SelectListItem é a combobox (Vai renderizar)
        public IEnumerable<SelectListItem> Status { get; set; }

        //SelectListItem é a combobox (Vai renderizar)
        //   public IEnumerable<SelectListItem> Tickets { get; set; }
    }
}
