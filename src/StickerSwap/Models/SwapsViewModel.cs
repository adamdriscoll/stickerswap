using StickerSwap.Data;
using System.Collections.Generic;
using System.Linq;

namespace StickerSwap.Models
{
    public class SwapsViewModel
    {
        public IEnumerable<Swap> ActiveRequests { get; set; }
        public IEnumerable<Swap> ActivePicks { get; set; }
        public IEnumerable<Swap> PastRequests { get; set; }
        public IEnumerable<Swap> PastPicks { get; set; }
        public bool Empty
        {
            get
            {
                return !ActiveRequests.Any() && !ActivePicks.Any() && !PastRequests.Any() && !PastPicks.Any();
            }
        }
    }
}
