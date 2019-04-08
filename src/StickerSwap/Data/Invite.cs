namespace StickerSwap.Data
{
    public class Invite
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string EmailAddress { get; set; }
        public string Key { get; set; }
    }
}
