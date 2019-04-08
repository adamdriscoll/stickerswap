using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StickerSwap.Data;
using StickerSwap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StickerSwap.Controllers
{
    [Authorize]
    public class SwapController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public SwapController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var swapsViewModel = new SwapsViewModel();

            var picks = _dbContext.Swaps.Include(m => m.Sticker).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.User.Id == userId && m.Status != SwapStatus.Complete).OrderByDescending(m => m.Date);
            var requests = _dbContext.Swaps.Include(m => m.Sticker).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.Sticker.User.Id == userId && m.Status != SwapStatus.Complete).OrderByDescending(m => m.Date);

            var pastPicks = _dbContext.Swaps.Include(m => m.Sticker).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.User.Id == userId && m.Status == SwapStatus.Complete).OrderByDescending(m => m.Date);
            var pastRequests = _dbContext.Swaps.Include(m => m.Sticker).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.Sticker.User.Id == userId && m.Status == SwapStatus.Complete).OrderByDescending(m => m.Date);

            swapsViewModel.ActivePicks = picks;
            swapsViewModel.ActiveRequests = requests;
            swapsViewModel.PastPicks = pastPicks;
            swapsViewModel.PastRequests = pastRequests;

            return View("Index", swapsViewModel);
        }

        [Route("{id}/shipping")]
        public IActionResult Shipping([FromRoute]long id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = _dbContext.Swaps.Include(m => m.Sticker).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Only can see address if processing
            if (order.Status != SwapStatus.Processing)
            {
                return BadRequest();
            }

            // Only the shipper can see the address
            if (order.Sticker.User.Id != userId)
            {
                return BadRequest();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SwapViewModel viewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var product = _dbContext.Stickers.FirstOrDefault(m => m.Id == viewModel.StickerId);
            var user = _dbContext.Users.First(m => m.Id == userId);

            user.Credits -= viewModel.Quantity;
            product.Quantity -= viewModel.Quantity;

            var order = new Swap
            {
                Date = DateTime.UtcNow,
                User = user,
                Sticker = product,
                Status = SwapStatus.Processing,
                Quantity = viewModel.Quantity,
                Credits = viewModel.Quantity * product.Credits
            };

            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();

            return View();
        }

        [HttpPost]
        [Route("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute]long id, SwapStatus swapStatus)
        {
            var swap = _dbContext.Swaps.Include(m => m.User).Include(m => m.Sticker).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == id);

            if (swap == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Can only transition to correct state
            switch(swap.Status)
            {
                case SwapStatus.Processing:
                    if (swapStatus != SwapStatus.Cancelled && swapStatus != SwapStatus.Shipped)
                    {
                        return BadRequest();
                    }
                    break;
                case SwapStatus.Shipped:
                    if (swapStatus != SwapStatus.Complete)
                    {
                        return BadRequest();
                    }
                    break;
            }

            // Only users that are part of this order can cancel it
            if (swap.Status == SwapStatus.Processing && swapStatus == SwapStatus.Cancelled)
            {
                if (swap.User.Id != userId || swap.Sticker.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            // Only the product author can ship the item
            if (swap.Status == SwapStatus.Processing && swapStatus == SwapStatus.Shipped)
            {
                if (swap.Sticker.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            // Only the order owner can complete the request
            if (swap.Status == SwapStatus.Shipped && swapStatus != SwapStatus.Complete)
            {
                if (swap.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            if (swapStatus == SwapStatus.Cancelled)
            {
                swap.User.Credits += swap.Credits;
                swap.Sticker.Quantity += swap.Credits;
            }

            if (swapStatus == SwapStatus.Complete)
            {
                swap.Sticker.User.Credits += swap.Credits;
            }

            swap.Status = swapStatus;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
