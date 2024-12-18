using AllupVol2.ViewModels;
using AllupVol2.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs=await _context.Products.Include(p=>p.Category).Include(p=>p.ProductImages).Select(p=>new GetProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Availability = p.Availability,
                CategoryName =p.Category.Name,
                Price = p.Price,
                ProductImages = p.ProductImages,
                DisCountPercentage = p.DisCountPercentage,
                DisCountPrice = p.DisCountPrice,
            }).ToListAsync();
            return View(productVMs);
        }
    }
}
