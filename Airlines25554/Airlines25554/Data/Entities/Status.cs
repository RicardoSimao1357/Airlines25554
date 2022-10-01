using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Status : IEntity
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "The field {0} is required")]
        public string StatusName { get; set; }
    }
}
