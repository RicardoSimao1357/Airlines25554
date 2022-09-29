﻿using Airlines25554.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class AirportViewModel 
    {
        public int CityId { get; set; }


        public int AirportId { get; set; }


        [Required]
        [Display(Name = "Airport")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
