using System;
using System.Collections.Generic;

namespace StickerSwap.Data
{
    public class Sticker
    {
        public long Id { get; set; }        
        public User User { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public int Quantity { get; set; }
        public int Credits { get; set; }
        public string BlobUri { get; set; }
        public byte[] Blob { get; set; }
        public string BlobMediaType { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public ICollection<StickerTag> StickerTags { get; set; }
    }
}
