using Microsoft.AspNetCore.Mvc;
using MvcHer.Services;
using MvcHer.Models;

namespace MvcHer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string? category = null, string? search = null)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(search))
            {
                products = await _productService.SearchProductsAsync(search);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                products = await _productService.GetProductsByCategoryAsync(category);
            }
            else
            {
                products = await _productService.GetActiveProductsAsync();
            }

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;
            ViewBag.SearchTerm = search;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductJson(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }

            return Json(new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                description = product.Description,
                imageUrl = product.ImageUrl,
                category = product.Category,
                stockQuantity = product.StockQuantity
            });
        }
    }
}
