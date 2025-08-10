using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MvcHer.Data;
using MvcHer.Services;
using MvcHer.Models;

namespace MvcHer.Controllers
{
    public class AdminController : Controller
    {
        private readonly TeaShopDbContext _context;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IAboutUsService _aboutUsService;
        private readonly ITestimonialService _testimonialService;
        private readonly ISocialLinkService _socialLinkService;
        private readonly IBannerService _bannerService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(TeaShopDbContext context, IProductService productService, IOrderService orderService, IAboutUsService aboutUsService, ITestimonialService testimonialService, ISocialLinkService socialLinkService, IBannerService bannerService, ILogger<AdminController> logger)
        {
            _context = context;
            _productService = productService;
            _orderService = orderService;
            _aboutUsService = aboutUsService;
            _testimonialService = testimonialService;
            _socialLinkService = socialLinkService;
            _bannerService = bannerService;
            _logger = logger;
        }

        // Default Index action for /admin route
        public IActionResult Index()
        {
            // Check if user is already authenticated
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                // If authenticated, redirect to dashboard
                return RedirectToAction("Dashboard");
            }
            
            // If not authenticated, redirect to login
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                // Check against database
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == username && a.IsActive);

                if (admin != null)
                {
                    bool isPasswordValid = false;
                    try
                    {
                        // Try to verify with BCrypt
                        isPasswordValid = BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
                    }
                    catch (BCrypt.Net.SaltParseException)
                    {
                        // If BCrypt fails, check if it's a plain text password (for initial setup)
                        if (admin.PasswordHash == password)
                        {
                            // Update to hashed password
                            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                            await _context.SaveChangesAsync();
                            isPasswordValid = true;
                        }
                    }

                    if (isPasswordValid)
                    {
                        // Update last login
                        admin.LastLoginDate = DateTime.UtcNow;
                        await _context.SaveChangesAsync();

                        // Set session
                        HttpContext.Session.SetString("IsAdmin", "true");
                        HttpContext.Session.SetString("AdminUser", admin.Username);
                        HttpContext.Session.SetString("AdminId", admin.Id.ToString());
                        HttpContext.Session.SetString("AdminFullName", admin.FullName);
                        
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (in a real app, use proper logging)
                ViewBag.Error = "An error occurred during login. Please try again.";
                return View();
            }
            
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            // Get dashboard data
            var currentMonth = DateTime.UtcNow.AddDays(-30);
            var totalSales = await _orderService.GetTotalSalesAsync(currentMonth);
            var totalOrders = await _orderService.GetTotalOrdersAsync(currentMonth);
            var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");
            var recentOrders = await _orderService.GetRecentOrdersAsync(10);

            ViewBag.TotalSales = totalSales;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.RecentOrders = recentOrders;
            
            return View();
        }

        public async Task<IActionResult> Products(string filter = "active")
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            // Show active products by default, all products if filter=all
            var products = filter?.ToLower() == "all" 
                ? await _productService.GetAllProductsAsync()
                : await _productService.GetActiveProductsAsync();
                
            ViewBag.CurrentFilter = filter?.ToLower() ?? "active";
            ViewBag.ActiveCount = (await _productService.GetActiveProductsAsync()).Count();
            ViewBag.TotalCount = (await _productService.GetAllProductsAsync()).Count();
            
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile? productImage)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (productImage != null && productImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productImage.CopyToAsync(fileStream);
                        }

                        product.ImageUrl = "/img/" + uniqueFileName;
                    }
                    else
                    {
                        // Default image if none provided
                        product.ImageUrl = "/img/product-1.jpg";
                    }

                    // Set default values
                    product.IsActive = true;
                    product.CreatedAt = DateTime.UtcNow;
                    product.UpdatedAt = DateTime.UtcNow;

                    await _productService.CreateProductAsync(product);
                    TempData["Success"] = "Product created successfully!";
                    return RedirectToAction("Products");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error creating product: " + ex.Message;
                }
            }
            else
            {
                // Collect validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Validation errors: " + string.Join(", ", errors);
            }

            return RedirectToAction("Products");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(product);
                    TempData["Success"] = "Product updated successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating product: " + ex.Message;
                }
            }

            return RedirectToAction("Products");
        }

        [HttpGet]
        public async Task<IActionResult> ViewProduct(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["Error"] = "Product not found.";
                    return RedirectToAction("Products");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading product: " + ex.Message;
                return RedirectToAction("Products");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    TempData["Error"] = "Product not found.";
                    return RedirectToAction("Products");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading product: " + ex.Message;
                return RedirectToAction("Products");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? productImage)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload if new image is provided
                    if (productImage != null && productImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productImage.CopyToAsync(fileStream);
                        }

                        product.ImageUrl = "/img/" + uniqueFileName;
                    }

                    // Update timestamp
                    product.UpdatedAt = DateTime.UtcNow;

                    await _productService.UpdateProductAsync(product);
                    TempData["Success"] = "Product updated successfully!";
                    return RedirectToAction("Products");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error updating product: " + ex.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Validation errors: " + string.Join(", ", errors);
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                await _productService.DeleteProductAsync(id);
                TempData["Success"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting product: " + ex.Message;
            }

            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Orders()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }
            
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading orders: " + ex.Message;
                return View(new List<Order>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }

            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, status);
                TempData["Success"] = "Order status updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating order status: " + ex.Message;
            }

            return RedirectToAction("Orders");
        }

        public IActionResult Users()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }
            
            return View();
        }



        public IActionResult Settings()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }
            
            return View();
        }
        
        public async Task<IActionResult> ContactMessages()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }
            
            try
            {
                var messages = await _context.ContactMessages
                    .OrderByDescending(m => m.CreatedAt)
                    .ToListAsync();
                return View(messages);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading contact messages: " + ex.Message;
                return View(new List<ContactMessage>());
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Json(new { success = false, message = "Unauthorized" });
            }
            
            try
            {
                var message = await _context.ContactMessages.FindAsync(id);
                if (message != null)
                {
                    message.IsRead = true;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Message not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // About Us Management
        public async Task<IActionResult> AboutUs()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            var aboutUs = await _aboutUsService.GetAboutUsAsync();
            return View(aboutUs);
        }

        [HttpGet]
        public async Task<IActionResult> EditAboutUs()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            var aboutUs = await _aboutUsService.GetAboutUsAsync();
            return View(aboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(AboutUs aboutUs, IFormFile? imageFile, IFormFile? founderImageFile)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle main image upload
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = $"about-hero-{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(imageFile.FileName)}";
                        var filePath = Path.Combine("wwwroot/img", fileName);
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        
                        aboutUs.ImageUrl = $"~/img/{fileName}";
                    }

                    // Handle founder image upload
                    if (founderImageFile != null && founderImageFile.Length > 0)
                    {
                        var fileName = $"founder-{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(founderImageFile.FileName)}";
                        var filePath = Path.Combine("wwwroot/img", fileName);
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await founderImageFile.CopyToAsync(stream);
                        }
                        
                        aboutUs.FounderImageUrl = $"~/img/{fileName}";
                    }

                    if (aboutUs.Id == 0)
                    {
                        await _aboutUsService.CreateAboutUsAsync(aboutUs);
                        TempData["SuccessMessage"] = "About Us information created successfully!";
                    }
                    else
                    {
                        await _aboutUsService.UpdateAboutUsAsync(aboutUs);
                        TempData["SuccessMessage"] = "About Us information updated successfully!";
                    }

                    return RedirectToAction("AboutUs");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error saving About Us information: {ex.Message}";
                }
            }

            return View(aboutUs);
        }

        // Testimonial Management
        public async Task<IActionResult> Testimonials()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            var testimonials = await _testimonialService.GetAllTestimonialsAsync();
            return View(testimonials);
        }

        [HttpGet]
        public async Task<IActionResult> EditTestimonial(int? id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            if (id.HasValue)
            {
                var testimonial = await _testimonialService.GetTestimonialByIdAsync(id.Value);
                if (testimonial == null)
                {
                    TempData["ErrorMessage"] = "Testimonial not found.";
                    return RedirectToAction("Testimonials");
                }
                return View(testimonial);
            }

            return View(new Testimonial());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTestimonial(Testimonial testimonial, IFormFile? clientImageFile)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle client image upload
                    if (clientImageFile != null && clientImageFile.Length > 0)
                    {
                        // Validate file type
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        var fileExtension = Path.GetExtension(clientImageFile.FileName).ToLowerInvariant();
                        
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            TempData["ErrorMessage"] = "Please upload a valid image file (JPG, PNG, GIF, or WebP).";
                            return View(testimonial);
                        }
                        
                        // Validate file size (max 5MB)
                        if (clientImageFile.Length > 5 * 1024 * 1024)
                        {
                            TempData["ErrorMessage"] = "Image file size must be less than 5MB.";
                            return View(testimonial);
                        }
                        
                        var fileName = $"testimonial-{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                        var uploadsDir = Path.Combine("wwwroot", "img");
                        var filePath = Path.Combine(uploadsDir, fileName);
                        
                        // Create directory if it doesn't exist
                        Directory.CreateDirectory(uploadsDir);
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await clientImageFile.CopyToAsync(stream);
                        }
                        
                        testimonial.ClientImageUrl = $"/img/{fileName}";
                        
                        // Log successful upload
                        _logger.LogInformation("Image uploaded successfully for testimonial: {FileName}", fileName);
                    }
                    else if (testimonial.Id > 0)
                    {
                        // For updates, preserve existing image URL if no new image is uploaded
                        var existingTestimonial = await _testimonialService.GetTestimonialByIdAsync(testimonial.Id);
                        if (existingTestimonial != null && !string.IsNullOrEmpty(existingTestimonial.ClientImageUrl))
                        {
                            testimonial.ClientImageUrl = existingTestimonial.ClientImageUrl;
                        }
                    }

                    if (testimonial.Id == 0)
                    {
                        await _testimonialService.CreateTestimonialAsync(testimonial);
                        TempData["SuccessMessage"] = "Testimonial created successfully!";
                    }
                    else
                    {
                        await _testimonialService.UpdateTestimonialAsync(testimonial);
                        TempData["SuccessMessage"] = "Testimonial updated successfully!";
                    }

                    return RedirectToAction("Testimonials");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error saving testimonial: {ex.Message}";
                }
            }

            return View(testimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var result = await _testimonialService.DeleteTestimonialAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Testimonial deleted successfully" });
                }
                return Json(new { success = false, message = "Testimonial not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleTestimonialFeatured(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var result = await _testimonialService.ToggleFeaturedAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Featured status updated" });
                }
                return Json(new { success = false, message = "Testimonial not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleTestimonialApproval(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var result = await _testimonialService.ToggleApprovalAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Approval status updated" });
                }
                return Json(new { success = false, message = "Testimonial not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Social Links Management
        public async Task<IActionResult> SocialLinks()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            var socialLinks = await _socialLinkService.GetAllSocialLinksAsync();
            return View(socialLinks);
        }

        [HttpGet]
        public async Task<IActionResult> EditSocialLink(int? id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            if (id.HasValue)
            {
                var socialLink = await _socialLinkService.GetSocialLinkByIdAsync(id.Value);
                if (socialLink == null)
                {
                    TempData["ErrorMessage"] = "Social link not found.";
                    return RedirectToAction("SocialLinks");
                }
                return View(socialLink);
            }

            return View(new SocialLink());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSocialLink(SocialLink socialLink)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                try
                {
                    if (socialLink.Id == 0)
                    {
                        await _socialLinkService.CreateSocialLinkAsync(socialLink);
                        TempData["SuccessMessage"] = "Social link created successfully!";
                    }
                    else
                    {
                        await _socialLinkService.UpdateSocialLinkAsync(socialLink);
                        TempData["SuccessMessage"] = "Social link updated successfully!";
                    }

                    return RedirectToAction("SocialLinks");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error saving social link: {ex.Message}";
                }
            }

            return View(socialLink);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSocialLink(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var result = await _socialLinkService.DeleteSocialLinkAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Social link deleted successfully" });
                }
                return Json(new { success = false, message = "Social link not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSocialLinkStatus(int id)
        {
            if (HttpContext.Session.GetString("AdminId") == null)
                return Json(new { success = false, message = "Unauthorized" });

            try
            {
                var result = await _socialLinkService.ToggleActiveStatusAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Status updated successfully" });
                }
                return Json(new { success = false, message = "Social link not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        #region Banner Management

        [HttpGet]
        public async Task<IActionResult> Banners()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var banners = await _bannerService.GetAllBannersAsync();
            return View(banners);
        }

        [HttpGet]
        public IActionResult CreateBanner()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            return View(new Banner());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBanner(Banner banner, IFormFile? imageFile)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    banner.ImageUrl = "/images/banners/" + uniqueFileName;
                }

                if (ModelState.IsValid)
                {
                    var success = await _bannerService.CreateBannerAsync(banner);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Banner created successfully!";
                        return RedirectToAction("Banners");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to create banner.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating banner: {ex.Message}";
            }

            return View(banner);
        }

        [HttpGet]
        public async Task<IActionResult> EditBanner(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null)
            {
                TempData["ErrorMessage"] = "Banner not found.";
                return RedirectToAction("Banners");
            }

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(Banner banner, IFormFile? imageFile)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    banner.ImageUrl = "/images/banners/" + uniqueFileName;
                }

                if (ModelState.IsValid)
                {
                    var success = await _bannerService.UpdateBannerAsync(banner);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Banner updated successfully!";
                        return RedirectToAction("Banners");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update banner.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating banner: {ex.Message}";
            }

            return View(banner);
        }

        [HttpGet]
        public async Task<IActionResult> ViewBanner(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null)
            {
                TempData["ErrorMessage"] = "Banner not found.";
                return RedirectToAction("Banners");
            }

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            try
            {
                var success = await _bannerService.DeleteBannerAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Banner deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete banner.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting banner: {ex.Message}";
            }

            return RedirectToAction("Banners");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBannerStatus(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            try
            {
                var success = await _bannerService.ToggleBannerStatusAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Banner status updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update banner status.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating banner status: {ex.Message}";
            }

            return RedirectToAction("Banners");
        }

        #endregion
    }
}
