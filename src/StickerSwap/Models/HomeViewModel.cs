using StickerSwap.Data;
using System.Collections.Generic;

namespace StickerSwap.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Sticker> NewStickers { get; set; }
        public IEnumerable<Sticker> RecentSwaps { get; set; }
    }
}
