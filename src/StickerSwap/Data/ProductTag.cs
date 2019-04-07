using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class ProductTag
    {
        public long Id { get; set; }
        public Product Product { get; set; }
        public Tag Tag { get; set; }
    }
}
