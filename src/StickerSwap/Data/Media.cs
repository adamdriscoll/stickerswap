using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class Media
    {
        public long Id { get; set; }
        public string AlternateText { get; set; }
        public string BlobUri { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
