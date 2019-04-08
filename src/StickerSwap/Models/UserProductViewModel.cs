using StickerSwap.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Models
{
    public class UserProductViewModel
    {
        public IEnumerable<Product> Picks { get; set; }
        public IEnumerable<Product> Shares { get; set; }
    }
}
