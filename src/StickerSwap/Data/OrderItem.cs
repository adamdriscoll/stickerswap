using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class OrderItem
    {
        public long Id { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
