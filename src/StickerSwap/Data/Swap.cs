using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace StickerSwap.Data
{
    public class Swap
    {
        public long Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public SwapStatus Status { get; set; }
        public Sticker Sticker { get; set; }
        public int Quantity { get; set; }
        public int Credits { get; set; }
    }
}
