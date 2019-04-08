using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Models
{
    public class EditProductViewModel
    {
        [Display(Name = "Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [StringLength(1024, MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Height (inches)")]
        public float Height { get; set; }

        [Display(Name = "Width (inches)")]
        public float Width { get; set; }

        //[Required]
        //[Display(Name = "Tags")]
        //public string[] Tags { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
