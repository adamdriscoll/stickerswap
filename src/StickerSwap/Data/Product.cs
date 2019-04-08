using System;

namespace StickerSwap.Data
{
    public class Product
    {
        public long Id { get; set; }        
        public User User { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public int Quantity { get; set; }
        public string BlobUri { get; set; }
        public byte[] Blob { get; set; }
        public string BlobMediaType { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
}
