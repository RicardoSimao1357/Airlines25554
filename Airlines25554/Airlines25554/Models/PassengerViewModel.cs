using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class PassengerViewModel 
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Select a flight!")]
        public int FlightId { get; set; }

        public int TicketId { get; set; }

        [Required(ErrorMessage = "Email is mandatory!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name is mandatory!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is mandatory!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Passport number is mandatory!")]
        [Display(Name = "Passport Number")]
        public string PassportId { get; set; }

        public IEnumerable<SelectListItem> Classes { get; set; }

        public List<Ticket> TotalSeatsList { get; set; }

        public string Seat { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }
        public string Class { get; set; }

    }
}
