using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class Product
    {
        public long Id { get; set; }        
        public User User { get; set; }
        public ProductType ProductType { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public int Quantity { get; set; }
        public int Views { get; set; }
        public string BlobUri { get; set; }
        public byte[] Blob { get; set; }
        public string BlobMediaType { get; set; }
    }
}
