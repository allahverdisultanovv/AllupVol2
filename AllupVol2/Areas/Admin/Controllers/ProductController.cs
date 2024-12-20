using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using AllupVol2.Models;
using AllupVol2.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products
            .Include(p => p.Category)
            .Select(p =>
            new GetProductVM()
            {
                Name = p.Name,
                Availability = p.Availability,
                CategoryName = p.Category.Name,
                Price = p.Price,
                Id = p.Id,
            }
            ).ToListAsync();
            return View(productVMs);
        }
        public async Task<IActionResult> Create()
        {
            CreateProductVM createVM = new()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(createVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createVM)
        {
            createVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {return View(createVM);
            }
            bool result= createVM.Categories.Any(c=>c.Id==createVM.CategoryId);
            if (!result) { ModelState.AddModelError(nameof(createVM.CategoryId), "Category does not found"); return View(createVM); };
            if (!createVM.MainPhoto.CheckFileType("image/"))
            {
                ModelState.AddModelError(nameof(createVM.MainPhoto), "File Type is incoorrect");
                return View(createVM);
            }
            if (!createVM.MainPhoto.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(createVM.MainPhoto),"File Size is incoorrect");
                return View(createVM);
            }
            if (!createVM.HoverPhoto.CheckFileType("image/"))
            {
                ModelState.AddModelError(nameof(createVM.HoverPhoto), "File Type is incoorrect");
                return View(createVM);
            }
            if (!createVM.HoverPhoto.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(createVM.HoverPhoto), "File Size is incoorrect");
                return View(createVM);
            }
            ProductImage hover = new ProductImage()
            {
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                ImageURL = await createVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images"),
                IsPrimary = false,

            };
            ProductImage main = new ProductImage()
            {
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                ImageURL = await createVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images"),
                IsPrimary = true,

            };
            Product product = new()
            {
                Name = createVM.Name,
                Availability = createVM.Availability,
                Description = createVM.Description,
                CategoryId = createVM.CategoryId.Value,
                CreatedAt = DateTime.Now,
                Price = createVM.Price.Value,
                ProductCode = createVM.ProductCode,
                DisCountPrice=createVM.Price.Value-(createVM.Price.Value *createVM.DisCountPercentage/100),
                Title = createVM.Title,
                Tax = createVM.Tax.Value,
                IsDeleted = false,
                ProductImages = new List<ProductImage>() { hover, main }

            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product is null) return NotFound();
            UpdateProductVM productVM = new UpdateProductVM()
            {
                Name = product.Name,
                Availability = product.Availability,
                Description = product.Description,
                Categories=await _context.Categories.ToListAsync(),
                DisCountPercentage = product.DisCountPercentage,
                CategoryId = product.CategoryId,
                Price = product.Price,
                ProductCode = product.ProductCode,
                Title = product.Title,
                Tax = product.Tax,
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateProductVM updateVM)
        {
            if (id is null || id < 1) return BadRequest();
            updateVM.Categories = await _context.Categories.ToListAsync();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            bool result = updateVM.Categories.Any(c => c.Id == updateVM.CategoryId);
            if (!result) { ModelState.AddModelError(nameof(updateVM.CategoryId), "Category does not found"); return View(updateVM); };

             product.Name=updateVM.Name;
             product.Availability=updateVM.Availability;
            product.Description=updateVM.Description;
            product.DisCountPercentage=updateVM.DisCountPercentage.Value;
            product.CategoryId=updateVM.CategoryId.Value;
            product.Price=updateVM.Price.Value;
            product.ProductCode = updateVM.ProductCode;
            product.Title=updateVM.Title;
            product.Tax=updateVM.Tax.Value;
            product.Category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==updateVM.CategoryId);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
