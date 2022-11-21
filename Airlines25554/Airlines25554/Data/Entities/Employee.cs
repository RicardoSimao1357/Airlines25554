using System;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string DocumentId { get; set; }

        public User User { get; set; }


        [Display(Name = "Image")]

        public Guid ImageId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public string ImageFullPath => ImageId == Guid.Empty
       ? $"https://airlines25554tpsi.blob.core.windows.net/noimage/noimage.png"
       : $"https://airlines25554tpsi.blob.core.windows.net/users/{ImageId}";
    }
}
