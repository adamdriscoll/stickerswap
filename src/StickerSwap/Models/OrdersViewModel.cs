using StickerSwap.Data;
using System.Collections.Generic;

namespace StickerSwap.Models
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> ActiveRequests { get; set; }
        public IEnumerable<Order> ActivePicks { get; set; }
        public IEnumerable<Order> PastRequests { get; set; }
        public IEnumerable<Order> PastPicks { get; set; }
    }
}
