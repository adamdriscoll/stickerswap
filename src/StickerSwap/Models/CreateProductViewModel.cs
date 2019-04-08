﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StickerSwap.Models
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(1024, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Height (inches)")]
        public float Height { get; set; }

        [Required]
        [Display(Name = "Width (inches)")]
        public float Width { get; set; }

        //[Required]
        //[Display(Name = "Tags")]
        //public string[] Tags { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
