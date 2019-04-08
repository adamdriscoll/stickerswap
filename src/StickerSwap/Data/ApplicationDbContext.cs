using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StickerSwap.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sticker> Stickers { get; set; }
        public DbSet<Swap> Swaps { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<StickerTag> StickerTags { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Invite> Invites { get; set; }
        
    }
}
