using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcHer.Models;
using MvcHer.Services;
using MvcHer.Data;

namespace MvcHer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly TeaShopDbContext _context;
        private readonly IAboutUsService _aboutUsService;
        private readonly ITestimonialService _testimonialService;
        private readonly ISocialLinkService _socialLinkService;
        private readonly IBannerService _bannerService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, TeaShopDbContext context, IAboutUsService aboutUsService, ITestimonialService testimonialService, ISocialLinkService socialLinkService, IBannerService bannerService)
        {
            _logger = logger;
            _productService = productService;
            _context = context;
            _aboutUsService = aboutUsService;
            _testimonialService = testimonialService;
            _socialLinkService = socialLinkService;
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            // Get featured products for homepage
            var featuredProducts = await _productService.GetActiveProductsAsync();
            ViewBag.FeaturedProducts = featuredProducts.Take(4).ToList();
            
            // Get active banners for homepage slider
            var banners = await _bannerService.GetActiveBannersAsync();
            ViewBag.Banners = banners;

            // Get active testimonials for homepage
            var testimonials = await _testimonialService.GetActiveTestimonialsAsync();
            ViewBag.Testimonials = testimonials;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      

        public async Task<IActionResult> Testimonial()
        {
            _logger.LogInformation("Testimonial action called");
            var testimonials = await _testimonialService.GetActiveTestimonialsAsync();
            _logger.LogInformation("Retrieved {Count} testimonials for view", testimonials.Count);
            
            if (testimonials.Count == 0)
            {
                _logger.LogWarning("No testimonials found in database");
            }
            
            return View(testimonials);
        }

        public IActionResult Contact()
        {
            return View(new ContactMessage());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage contactMessage)
        {
            // Debug logging
            _logger.LogInformation("Contact form submitted - Name: {Name}, Email: {Email}, Subject: {Subject}", 
                contactMessage?.Name, contactMessage?.Email, contactMessage?.Subject);
            
            if (ModelState.IsValid)
            {
                try
                {
                    contactMessage.CreatedAt = DateTime.UtcNow;
                    contactMessage.IsRead = false;
                    
                    _logger.LogInformation("Adding contact message to database");
                    _context.ContactMessages.Add(contactMessage);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Contact message saved successfully");
                    
                    TempData["Success"] = "Thank you for your message! We'll get back to you soon.";
                    
                    // Clear the form by redirecting
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving contact message");
                    TempData["Error"] = "Sorry, there was an error sending your message. Please try again.";
                }
            }
            else
            {
                // Debug model state errors
                foreach (var error in ModelState)
                {
                    _logger.LogWarning("ModelState Error - Key: {Key}, Errors: {Errors}", 
                        error.Key, string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage)));
                }
                TempData["Error"] = "Please check the form and fill in all required fields correctly.";
            }
            
            return View(contactMessage);
        }

        public IActionResult Features()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }



        public IActionResult Services()
        {
            return View();
        }

        // Test methods for form debugging
        public IActionResult TestForms()
        {
            return View(new ContactMessage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestContact(ContactMessage contactMessage)
        {
            _logger.LogInformation("TestContact form submitted - Name: {Name}, Email: {Email}, Subject: {Subject}", 
                contactMessage?.Name, contactMessage?.Email, contactMessage?.Subject);
            
            if (ModelState.IsValid)
            {
                try
                {
                    contactMessage.CreatedAt = DateTime.UtcNow;
                    contactMessage.IsRead = false;
                    
                    _logger.LogInformation("Adding test contact message to database");
                    _context.ContactMessages.Add(contactMessage);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Test contact message saved successfully with ID: {Id}", contactMessage.Id);
                    
                    TempData["Success"] = "✅ TEST SUCCESS: Contact form is working! Message saved to database with ID: " + contactMessage.Id;
                    return RedirectToAction("TestForms");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving test contact message");
                    TempData["Error"] = "❌ TEST FAILED: Database error - " + ex.Message;
                }
            }
            else
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("TestContact validation failed: {Errors}", errors);
                TempData["Error"] = "❌ TEST FAILED: Validation errors - " + errors;
            }
            
            return View("TestForms", contactMessage);
        }

        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // Test database connection
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    TempData["Error"] = "❌ DATABASE TEST FAILED: Cannot connect to database";
                    return RedirectToAction("TestForms");
                }

                // Test ContactMessages table
                var messageCount = await _context.ContactMessages.CountAsync();
                var recentMessages = await _context.ContactMessages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(5)
                    .Select(m => new { m.Id, m.Name, m.Subject, m.CreatedAt })
                    .ToListAsync();

                // Test Testimonials table
                var testimonialCount = await _context.Testimonials.CountAsync();
                var testimonialDetails = await _context.Testimonials
                    .Select(t => new { t.Id, t.ClientName, t.IsActive, t.IsApproved, t.IsFeatured })
                    .ToListAsync();

                var result = $"✅ DATABASE TEST SUCCESS: Connected! ContactMessages: {messageCount} records. " +
                           $"Testimonials: {testimonialCount} records. " +
                           $"Testimonial details: {string.Join(", ", testimonialDetails.Select(t => $"ID:{t.Id} ({t.ClientName}, Active:{t.IsActive}, Approved:{t.IsApproved}, Featured:{t.IsFeatured})"))}";

                TempData["Success"] = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database test failed");
                TempData["Error"] = "❌ DATABASE TEST FAILED: " + ex.Message;
            }

            return RedirectToAction("TestForms");
        }

        public async Task<IActionResult> AddUniqueBanner()
        {
            try
            {
                // Check if this unique banner already exists
                var existingBanner = await _context.Banners
                    .FirstOrDefaultAsync(b => b.Title == "Premium Tea Collection 2025");

                if (existingBanner != null)
                {
                    TempData["Info"] = "✅ Unique banner already exists in database!";
                    return RedirectToAction("Index");
                }

                // Create a unique banner matching the screenshots
                var uniqueBanner = new Banner
                {
                    Title = "Organic &",
                    Subtitle = "Quality Tea Production",
                    Description = "Welcome to TEA House",
                    ImageUrl = "/img/tea-leaves-bg.jpg",
                    ButtonText = "Explore More",
                    ButtonUrl = "/Home/Products",
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.Banners.Add(uniqueBanner);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Unique banner created successfully: {Title}", uniqueBanner.Title);
                TempData["Success"] = $"✅ Unique banner '{uniqueBanner.Title}' added successfully to database!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create unique banner");
                TempData["Error"] = "❌ Failed to create unique banner: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSocialLinks()
        {
            try
            {
                var socialLinks = await _socialLinkService.GetActiveSocialLinksAsync();
                var result = socialLinks.Select(s => new
                {
                    id = s.Id,
                    platform = s.Platform,
                    displayName = s.DisplayName,
                    url = s.Url,
                    iconClass = s.IconClass,
                    color = s.Color,
                    displayOrder = s.DisplayOrder
                }).OrderBy(s => s.displayOrder).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving social links");
                return Json(new List<object>());
            }
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
