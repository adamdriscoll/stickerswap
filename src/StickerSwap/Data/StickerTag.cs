using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class StickerTag
    {
        public long Id { get; set; }
        public Sticker Sticker { get; set; }
        public Tag Tag { get; set; }
    }
}
