using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Ticket : IEntity
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

        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

    }
}
