using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Airport : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Airport")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

      
    }
}
