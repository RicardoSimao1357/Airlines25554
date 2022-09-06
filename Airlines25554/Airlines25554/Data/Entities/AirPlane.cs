using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class AirPlane
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Registration { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int EconomySeats { get; set; }

        public int ExecutiveSeats { get; set; }

        public int FirstClassSeats { get; set; }
    }
}
