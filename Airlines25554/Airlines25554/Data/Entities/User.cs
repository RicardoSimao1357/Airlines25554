﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Data.Entities
{
    public class User : IdentityUser
    {
        //[MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        //public string FirstName { get; set; }

        //[MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        //public string LastName { get; set; }

        //[Display(Name = "Full Name")]
        //public string FullName => $"{FirstName} {LastName}";



    }
}
