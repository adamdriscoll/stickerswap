using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StickerSwap.Data;

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
        [Display(Name = "Sticker Type")]
        public ProductType ProductType { get; set; }

        //[Required]
        //[Display(Name = "Tags")]
        //public string[] Tags { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
