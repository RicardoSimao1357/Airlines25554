﻿using Airlines25554.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airlines25554.Models
{
    public class CountryViewModel : Country
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


       
    }
}
