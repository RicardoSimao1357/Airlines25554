using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class PassengerViewModel
    {
        [Required(ErrorMessage = "Select a flight!")]
        public int FlightId { get; set; }

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

        public List<string> SeatIsAvailable { get; set; }

        public int Seat { get; set; }

        public int EconomicSeats { get; set; }  

        public int ExecutiveSeats { get; set; }

        public int FirstClassSeats { get; set; }

        [Required(ErrorMessage = "Select a class!")]
        [Range(1, 2, ErrorMessage = "Select a class!")]
        public int Class { get; set; }

    }
}
