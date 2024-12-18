using AllupVol2.DAL;
using AllupVol2.Models;
using AllupVol2.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(SortType sortType)
        {
            List<Product> products = null;
            switch (sortType)
            {
                case SortType.Name:
                    products = await _context.Products.OrderBy(p => p.Name).Take(8).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).ToListAsync();
                    break;
                case SortType.Date:
                    products = await _context.Products.OrderByDescending(p => p.CreatedAt).Take(8).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).ToListAsync();

                    break;
                case SortType.Price:
                    products = await _context.Products.OrderByDescending(p => p.Price).Take(8).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).ToListAsync();

                    break;
                default:
                    break;
            }

            return View(products);
        }
    }
}
