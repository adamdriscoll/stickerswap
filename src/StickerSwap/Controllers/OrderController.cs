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
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var ordersViewModel = new OrdersViewModel();

            var picks = _dbContext.Orders.Include(m => m.Product).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.User.Id == userId && m.Status != OrderStatus.Complete).OrderByDescending(m => m.OrderDate);
            var requests = _dbContext.Orders.Include(m => m.Product).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.Product.User.Id == userId && m.Status != OrderStatus.Complete).OrderByDescending(m => m.OrderDate);

            var pastPicks = _dbContext.Orders.Include(m => m.Product).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.User.Id == userId && m.Status == OrderStatus.Complete).OrderByDescending(m => m.OrderDate);
            var pastRequests = _dbContext.Orders.Include(m => m.Product).ThenInclude(m => m.User).Include(m => m.User).Where(m => m.Product.User.Id == userId && m.Status == OrderStatus.Complete).OrderByDescending(m => m.OrderDate);

            ordersViewModel.ActivePicks = picks;
            ordersViewModel.ActiveRequests = requests;
            ordersViewModel.PastPicks = pastPicks;
            ordersViewModel.PastRequests = pastRequests;

            return View(ordersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var product = _dbContext.Products.FirstOrDefault(m => m.Id == orderViewModel.ProductId);
            var user = _dbContext.Users.First(m => m.Id == userId);

            user.Credits -= orderViewModel.Quantity;
            product.Quantity -= orderViewModel.Quantity;

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Product = product,
                Status = OrderStatus.Processing,
                Quantity = orderViewModel.Quantity
            };

            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();

            return View();
        }
    }
}
