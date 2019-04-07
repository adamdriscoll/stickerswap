using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class Notification
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public bool Dismissed { get; set; }
        public long ResourceId { get; set; }
        public ResourceType ResourceType { get; set; }
        public User User { get; set; }
    }
}
