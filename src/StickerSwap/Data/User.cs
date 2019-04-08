using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Data
{
    public class User : IdentityUser
    {
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Credits { get; set; }
        public ICollection<Swap> Swaps { get; set; }
        public ICollection<Sticker> Stickers { get; set; }
        public ICollection<Notification> Notification { get; set; }
    }
}
