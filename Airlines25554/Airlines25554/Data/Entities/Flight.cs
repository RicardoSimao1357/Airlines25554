using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Flight : IEntity
    {
     
        public int Id { get; set; }

        [Display(Name = "Flight Nº")]
        [Required(ErrorMessage = "The field {0} is required")]
        public string FlightNumber { get; set; }    

        public AirPlane AirPlane { get; set; }  
        
        public Airport From { get; set; }

        public Airport To{ get; set; }


        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public Status Status { get; set; }

        public int BusyEconomicSeats { get; set; }

        public int BusyExecutiveSeats { get; set; }

        public int BusyFirstClassSeats { get; set; }


    }
}
