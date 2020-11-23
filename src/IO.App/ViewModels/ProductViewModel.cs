﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace IO.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} - Required field")]
        [StringLength(200, ErrorMessage = "The field {0} must be between {2} e {1} characteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} - Required field")]
        [StringLength(1000, ErrorMessage = "The field {0} must be between {2} e {1} characteres", MinimumLength = 2)]
        public string Description { get; set; }

        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "{0} - Required field")]
        public decimal Value { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateRegister { get; set; }

        [DisplayName("Activ?")]
        public bool Activ { get; set; }

        /* Entity Framework Relations */
        public ProviderViewModel Provider { get; set; }
    }
}
