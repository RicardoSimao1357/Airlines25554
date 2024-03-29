﻿using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class TicketPurchased : IEntity
    {
        public int Id { get; set; }

        // Campo de preenchimento obrigatório
        [Required(ErrorMessage = "The field {0} is required")]
        public User User { get; set; }

        // Campo de preenchimento obrigatório
        [Required(ErrorMessage = "The field {0} is required")]
        public Flight Flight { get; set; }

        // Campo de preenchimento obrigatório
        [Required(ErrorMessage = "The field {0} is required")]
        public string Class { get; set; }

        // Campo de preenchimento obrigatório, o lugar vai corresponder ao lugar no avião
        [Required(ErrorMessage = "The field {0} is required")]
        public string Seat { get; set; }
    }
}
