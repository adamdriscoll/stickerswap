using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StickerSwap.Data;
using StickerSwap.Models;
using SixLabors.ImageSharp.Processing;


namespace StickerSwap.Controllers
{
    [Route("sticker")]
    public class StickerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StickerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("edit/{id}")]
        [Authorize]
        public IActionResult Update([FromRoute]long id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var sticker = _dbContext.Stickers.FirstOrDefault(m => m.User.Id == userId && m.Id == id);

            if (sticker == null)
            {
                return NotFound();
            }

            var viewModel = new EditStickerViewModel
            {
                Id = sticker.Id,
                Credits = sticker.Credits,
                Description = sticker.Description,
                Height = sticker.Height,
                Width = sticker.Width,
                Quantity = sticker.Quantity,
                Title = sticker.Title
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [Route("save/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSticker([FromRoute]long id, EditStickerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Not valid");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sticker = _dbContext.Stickers.FirstOrDefault(m => m.Id == id && m.User.Id == userId);

            if (sticker == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(viewModel.Title))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(viewModel.Description))
            {
                return BadRequest();
            }

            if (viewModel.Height <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Width <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Quantity <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Credits <= 0)
            {
                return BadRequest();
            }

            sticker.Height = viewModel.Height;
            sticker.Width = viewModel.Width;
            sticker.Description = viewModel.Description;
            sticker.Title = viewModel.Title;
            sticker.Quantity = viewModel.Quantity;
            sticker.Credits = viewModel.Credits;

            if (viewModel.Image != null)
            {
                sticker.BlobMediaType = viewModel.Image.ContentType;

                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.Image.CopyToAsync(memoryStream);
                    sticker.Blob = memoryStream.ToArray();
                }

                ResizeImage(sticker);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { id = sticker.Id } );
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateStickerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Not Valid");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.First(m => m.Id == userId);

            if (string.IsNullOrEmpty(viewModel.Title))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(viewModel.Description))
            {
                return BadRequest();
            }

            if (viewModel.Height <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Width <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Quantity <= 0)
            {
                return BadRequest();
            }

            if (viewModel.Credits <= 0)
            {
                return BadRequest();
            }

            var product = new Sticker
            {
                Created = DateTime.UtcNow,
                Description = viewModel.Description,
                Title = viewModel.Title,
                Height = viewModel.Height,
                Width = viewModel.Width,
                Quantity = viewModel.Quantity,
                Credits = viewModel.Credits,
                User = user,
                BlobMediaType = viewModel.Image.ContentType
            };

            using (var memoryStream = new MemoryStream())
            {
                await viewModel.Image.CopyToAsync(memoryStream);
                product.Blob = memoryStream.ToArray();

                ResizeImage(product);
            }

            _dbContext.Add(product);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { id = product.Id });
        }

        private void ResizeImage(Sticker sticker)
        {
            using (var image = SixLabors.ImageSharp.Image.Load(sticker.Blob))
            {
                var size = 500f * 500f;
                var width = Math.Sqrt(size * (image.Width / (float)image.Height));
                var height = size / width;

                image.Mutate(x => x.Resize((int)width, (int)height));
                using (var stream = new MemoryStream())
                {
                    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                    image.Save(stream, encoder);
                    sticker.Blob = stream.ToArray();
                    sticker.BlobMediaType = "image/png";
                }
            }
        }

        [Route("{id}")]
        public IActionResult Index([FromRoute]int id)
        {
            var product = _dbContext.Stickers.Include(m => m.User).FirstOrDefault(m => m.Id == id);

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _dbContext.Users.FirstOrDefault(m => m.Id == userId);

                ViewData["credits"] = user.Credits;
            }

            if (product == null)
            {
                return NotFound();
            }

            return View("Single", product);
        }

        [Route("image/{id}")]
        public IActionResult Image([FromRoute]long id)
        {
            var media = _dbContext.Stickers.FirstOrDefault(m => m.Id == id);

            if (media == null)
            {
                return NotFound();
            }

            return File(media.Blob, media.BlobMediaType);
        }

        [Route("user/{id}")]
        public IActionResult UserSticker([FromRoute]string id)
        {
            var user = _dbContext.Users.Include(m => m.Stickers).Include(m => m.Swaps).ThenInclude(m => m.Sticker).FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserStickerViewModel
            {
                Picks = user.Swaps.Select(m => m.Sticker),
                Shares = user.Stickers
            };

            return View("User", viewModel);
        }

        [Route("user")]
        public IActionResult UserSticker()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.Include(m => m.Stickers).Include(m => m.Swaps).ThenInclude(m => m.Sticker).FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserStickerViewModel
            {
                Picks = user.Swaps.Select(m => m.Sticker),
                Shares = user.Stickers
            };

            return View("User", viewModel);
        }

        [Route("search")]
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            var stickerCount = _dbContext.Stickers.Count(m => m.Title.Contains(searchViewModel.SearchText) || m.Description.Contains(searchViewModel.SearchText));

            var take = 20;
            var skip = 20 * searchViewModel.Page;

            var stickers = _dbContext.Stickers.Where(m => m.Title.Contains(searchViewModel.SearchText) || m.Description.Contains(searchViewModel.SearchText)).OrderByDescending(m => m.Created).Skip(skip).Take(take);

            searchViewModel.NumberOfPages = (stickerCount / take) + 1;
            searchViewModel.Stickers = stickers;

            return View(searchViewModel);
        }

    }
}