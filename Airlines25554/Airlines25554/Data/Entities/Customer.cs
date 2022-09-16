using System;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class Customer : IEntity
    {
        public int Id { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PassportId { get; set; }  

        public User User { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
       ? $"https://airlines25554.blob.core.windows.net/noimage/noimage.png"
       : $"https://airlines25554.blob.core.windows.net/airplanes/{ImageId}";


    }
}
