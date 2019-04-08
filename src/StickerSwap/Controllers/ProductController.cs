using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StickerSwap.Data;
using StickerSwap.Models;

namespace StickerSwap.Controllers
{
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

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute]long id, CreateProductViewModel createProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("WTF");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.First(m => m.Id == userId);
            var product = _dbContext.Products.FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Height = createProductViewModel.Height;
            product.Width = createProductViewModel.Width;
            product.Description = createProductViewModel.Description;
            product.Title = createProductViewModel.Title;
            product.Quantity = createProductViewModel.Quantity;
            product.BlobMediaType = createProductViewModel.Image.ContentType;

            using (var memoryStream = new MemoryStream())
            {
                await createProductViewModel.Image.CopyToAsync(memoryStream);
                product.Blob = memoryStream.ToArray();
            }

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

        [Authorize]
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
                Height = createProductViewModel.Height, 
                Width = createProductViewModel.Width,
                Quantity = createProductViewModel.Quantity,
                User = user,
                BlobMediaType = createProductViewModel.Image.ContentType
            };

            using (var memoryStream = new MemoryStream())
            {
                await createProductViewModel.Image.CopyToAsync(memoryStream);
                product.Blob = memoryStream.ToArray();
            }

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

        [Route("{id}")]
        public IActionResult Index([FromRoute]int id)
        {
            var product = _dbContext.Products.FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                Title = product.Title,
                Description = product.Description,
                Quantity = product.Quantity
            };

            return View("Single", productViewModel);
        }

        [Route("image/{id}")]
        public IActionResult Image([FromRoute]long id)
        {
            var media = _dbContext.Products.FirstOrDefault(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            return File(media.Blob, media.BlobMediaType);
        }

        [Route("user/{id}")]
        public IActionResult UserProduct([FromRoute]string id)
        {
            var user = _dbContext.Users.Include(m => m.Products).Include(m => m.Orders).ThenInclude(m => m.Product).FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var productViewModel = new UserProductViewModel
            {
                Picks = user.Orders.Select(m => m.Product),
                Shares = user.Products
            };

            return View("User", productViewModel);
        }

        [Route("user")]
        public IActionResult UserProduct()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.Include(m => m.Products).Include(m => m.Orders).ThenInclude(m => m.Product).FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var productViewModel = new UserProductViewModel
            {
                Picks = user.Orders.Select(m => m.Product),
                Shares = user.Products
            };

            return View("User", productViewModel);
        }

    }
}