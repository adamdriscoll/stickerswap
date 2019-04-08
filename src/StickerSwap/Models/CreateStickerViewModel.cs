using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StickerSwap.Models
{
    public class CreateStickerViewModel
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
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Credit per Sticker")]
        [Range(1, 10)]
        public int Credits { get; set; }

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
