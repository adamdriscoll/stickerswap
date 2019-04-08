using StickerSwap.Data;
using System.Collections.Generic;

namespace StickerSwap.Models
{
    public class UserStickerViewModel
    {
        public IEnumerable<Sticker> Picks { get; set; }
        public IEnumerable<Sticker> Shares { get; set; }
    }
}
