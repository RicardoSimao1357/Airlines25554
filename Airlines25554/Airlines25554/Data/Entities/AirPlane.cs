using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class AirPlane : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name ="Model")]
        public string AirplaneModel { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Registration { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int EconomySeats { get; set; }

        public int ExecutiveSeats { get; set; }

        public int FirstClassSeats { get; set; }

        public User User { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return $"https://localhost:44345{ImageUrl.Substring(1)}";
            }
        }
    }
}
