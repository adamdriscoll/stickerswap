using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using StickerSwap.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StickerSwap.Controllers
{
    public class InviteController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;

        public InviteController(IEmailSender emailSender, ApplicationDbContext dbContext)
        {
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(Invite invite)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(m => m.Id == userId);
            invite.Key = Guid.NewGuid().ToString().Replace("-", string.Empty);
            invite.User = user;

            _dbContext.Invites.Add(invite);
            await _dbContext.SaveChangesAsync();

            await _emailSender.SendEmailAsync(invite.EmailAddress, "Sticker Swap Invitation", $"You have been invited to join Sticker Swap by {user.Email}. Swap stickers with your friends! Try it out today at <a href='https://stickerswap.io'>stickerswap.io</a>. Use the referral code {invite.Key} when signing up to spread some sticker love around.");

            return RedirectToAction("Index", new { id = invite.Id });
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public IActionResult Index(long id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var invite = _dbContext.Invites.FirstOrDefault(m => m.Id == id && m.User.Id == userId);

            return View(invite);
        }
    }
}
