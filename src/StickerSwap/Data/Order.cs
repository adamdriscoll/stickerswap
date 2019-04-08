using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace StickerSwap.Data
{
    public class Order
    {
        public long Id { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public Product Product { get; set; }
        public long Quantity { get; set; }
    }
}
