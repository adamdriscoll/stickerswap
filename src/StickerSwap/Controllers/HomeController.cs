using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StickerSwap.Data;
using StickerSwap.Models;

namespace StickerSwap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var newStickers = _dbContext.Stickers.Include(m => m.StickerTags).ThenInclude(m => m.Tag).OrderByDescending(m => m.Created).Take(10);
            var recentlySwapped = _dbContext.Swaps.OrderByDescending(m => m.Date).GroupBy(m => m.Sticker).Take(10).Select(m => m.Key).Include(m => m.StickerTags).ThenInclude(m => m.Tag);

            var viewModel = new HomeViewModel
            {
                NewStickers = newStickers,
                RecentSwaps = recentlySwapped
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
