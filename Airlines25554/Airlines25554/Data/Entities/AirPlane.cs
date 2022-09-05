using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class AirPlane
    {
        public int Id { get; set; }

        public string Registration { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int EconomySeats { get; set; }

        public int ExecutiveSeats { get; set; }

        public int FirstClassSeats { get; set; }
    }
}
