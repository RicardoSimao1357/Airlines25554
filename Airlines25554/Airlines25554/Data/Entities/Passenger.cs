using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Passenger : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }


        public string Email { get; set; }   

        public string PassportId { get; set; }

        public User User { get; set; }

    }
}
