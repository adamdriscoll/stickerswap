using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StickerSwap.Data;
using StickerSwap.Models;

namespace StickerSwap.Controllers
{
    [Authorize]
    [Route("sticker")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("WTF");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.First(m => m.Id == userId);

            var product = new Product
            {
                Created = DateTime.UtcNow,
                Description = createProductViewModel.Description,
                Title = createProductViewModel.Title,
                ProductType = createProductViewModel.ProductType,
                Quantity = createProductViewModel.Quantity,
                User = user
            };

            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();

            var productViewModel = new ProductViewModel
            {
                Title = product.Title,
                Description = product.Description,
                Quantity = product.Quantity
            };

            return View("Single", productViewModel);
        }

        //[Route("{id}")]
        //public IActionResult Index([FromRoute]int stickerId)
        //{

        //}
    }
}