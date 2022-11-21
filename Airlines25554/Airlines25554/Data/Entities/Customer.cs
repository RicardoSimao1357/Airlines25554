using System;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Customer : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string PassportId { get; set; }

        public User User { get; set; }

       
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
       ? $"https://airlines25554tpsi.blob.core.windows.net/noimage/noimage.png"
       : $"https://airlines25554tpsi.blob.core.windows.net/users/{ImageId}";


    }
}
