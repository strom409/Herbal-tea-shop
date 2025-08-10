using Microsoft.AspNetCore.Mvc;
using MvcHer.Services;
using MvcHer.Models;

namespace MvcHer.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;

        public StoreController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string? category = null, string? priceRange = null, string? sortBy = null, int page = 1)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(category) && category != "All Categories")
            {
                products = await _productService.GetProductsByCategoryAsync(category);
            }
            else
            {
                products = await _productService.GetActiveProductsAsync();
            }

            // Apply price filtering
            if (!string.IsNullOrEmpty(priceRange) && priceRange != "Price Range")
            {
                products = ApplyPriceFilter(products, priceRange);
            }

            // Apply sorting
            products = ApplySorting(products, sortBy);

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentPriceRange = priceRange;
            ViewBag.CurrentSort = sortBy;

            return View(products);
        }

        private IEnumerable<Product> ApplyPriceFilter(IEnumerable<Product> products, string priceRange)
        {
            return priceRange switch
            {
                "0-20" => products.Where(p => p.Price >= 0 && p.Price <= 20),
                "20-40" => products.Where(p => p.Price > 20 && p.Price <= 40),
                "40-60" => products.Where(p => p.Price > 40 && p.Price <= 60),
                "60+" => products.Where(p => p.Price > 60),
                _ => products
            };
        }

        private IEnumerable<Product> ApplySorting(IEnumerable<Product> products, string? sortBy)
        {
            return sortBy switch
            {
                "name" => products.OrderBy(p => p.Name),
                "price-low" => products.OrderBy(p => p.Price),
                "price-high" => products.OrderByDescending(p => p.Price),
                "rating" => products.OrderByDescending(p => p.Name), // Placeholder for rating
                _ => products.OrderBy(p => p.Name)
            };
        }
    }
}
