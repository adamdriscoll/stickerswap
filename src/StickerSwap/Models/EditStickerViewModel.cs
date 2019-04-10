using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Models
{
    public class EditStickerViewModel
    {
        [Required]
        public long Id { get; set; }

        [Display(Name = "Title")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [StringLength(1024, MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Height (inches)")]
        public float Height { get; set; }

        [Display(Name = "Width (inches)")]
        public float Width { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Credit per Sticker")]
        [Range(1, 10)]
        public int Credits { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }
    }
}
