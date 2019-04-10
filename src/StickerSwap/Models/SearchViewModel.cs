using StickerSwap.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StickerSwap.Models
{
    public class SearchViewModel
    {
        [Display(Name = "Find Stickers!")]
        public string SearchText { get; set; }
        public IEnumerable<Sticker> Stickers { get; set; }
        public int Page { get; set; }
        public int NumberOfPages { get; set; }
    }
}
