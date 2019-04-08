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
    [Route("swap")]
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

            return View("Index", ordersViewModel);
        }

        [Route("{id}/shipping")]
        public IActionResult Shipping([FromRoute]long id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = _dbContext.Orders.Include(m => m.Product).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Only can see address if processing
            if (order.Status != OrderStatus.Processing)
            {
                return BadRequest();
            }

            // Only the shipper can see the address
            if (order.Product.User.Id != userId)
            {
                return BadRequest();
            }

            return View(order);
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

        [HttpPost]
        [Route("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute]long id, OrderStatus orderStatus)
        {
            var order = _dbContext.Orders.Include(m => m.User).Include(m => m.Product).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Can only transition to correct state
            switch(order.Status)
            {
                case OrderStatus.Processing:
                    if (orderStatus != OrderStatus.Cancelled && orderStatus != OrderStatus.Shipped)
                    {
                        return BadRequest();
                    }
                    break;
                case OrderStatus.Shipped:
                    if (orderStatus != OrderStatus.Complete)
                    {
                        return BadRequest();
                    }
                    break;
            }

            // Only users that are part of this order can cancel it
            if (order.Status == OrderStatus.Processing && orderStatus == OrderStatus.Cancelled)
            {
                if (order.User.Id != userId || order.Product.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            // Only the product author can ship the item
            if (order.Status == OrderStatus.Processing && orderStatus == OrderStatus.Shipped)
            {
                if (order.Product.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            // Only the order owner can complete the request
            if (order.Status == OrderStatus.Shipped && orderStatus != OrderStatus.Complete)
            {
                if (order.User.Id != userId)
                {
                    return BadRequest();
                }
            }

            order.Status = orderStatus;
            await _dbContext.SaveChangesAsync();

            return Index();
        }

    }
}
