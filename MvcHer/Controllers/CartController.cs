using Microsoft.AspNetCore.Mvc;
using MvcHer.Services;
using MvcHer.Models;
using MvcHer.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MvcHer.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly TeaShopDbContext _context;
        private readonly ILogger<CartController> _logger;
        private readonly IOtpService _otpService;

        public CartController(IProductService productService, IOrderService orderService, TeaShopDbContext context, ILogger<CartController> logger, IOtpService otpService)
        {
            _productService = productService;
            _orderService = orderService;
            _context = context;
            _logger = logger;
            _otpService = otpService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();
            
            // Load product details for each cart item
            foreach (var item in cart)
            {
                item.Product = await _productService.GetProductByIdAsync(item.ProductId);
            }
            
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null || !product.IsActive)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            if (product.StockQuantity < quantity)
            {
                return Json(new { success = false, message = "Insufficient stock" });
            }

            // Get cart from session
            var cart = GetCartFromSession();
            
            // Check if product already exists in cart
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Product = product,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Save cart to session
            SaveCartToSession(cart);

            return Json(new { 
                success = true, 
                message = "Product added to cart",
                cartCount = cart.Sum(c => c.Quantity),
                cartTotal = cart.Sum(c => c.TotalPrice)
            });
        }

        [HttpPost]
        public IActionResult UpdateCartItem(int productId, int quantity)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                
                SaveCartToSession(cart);
            }

            return Json(new { 
                success = true,
                cartCount = cart.Sum(c => c.Quantity),
                cartTotal = cart.Sum(c => c.TotalPrice)
            });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoteCart dto)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(c => c.ProductId == dto.ProductId);
            
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
            }

            return Json(new { 
                success = true,
                cartCount = cart.Sum(c => c.Quantity),
                cartTotal = cart.Sum(c => c.TotalPrice)
            });
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var cart = GetCartFromSession();
            return Json(new {
                items = cart.Select(c => new {
                    productId = c.ProductId,
                    name = c.Product?.Name,
                    price = c.Product?.Price,
                    quantity = c.Quantity,
                    total = c.TotalPrice,
                    imageUrl = c.Product?.ImageUrl
                }),
                cartCount = cart.Sum(c => c.Quantity),
                cartTotal = cart.Sum(c => c.TotalPrice)
            });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCartFromSession();
            var count = cart.Sum(c => c.Quantity);
            return Json(new { count = count });
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            try
            {
                HttpContext.Session.Remove("Cart");
                return Json(new { success = true, message = "Cart cleared successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to clear cart" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Store");
            }

            // Check if OTP is verified
            var otpSession = HttpContext.Session.GetString("OtpSession");
            if (string.IsNullOrEmpty(otpSession))
            {
                // Redirect to OTP verification first
                return RedirectToAction("VerifyIdentity");
            }

            var otpData = JsonSerializer.Deserialize<OtpSession>(otpSession);
            if (!otpData.IsVerified)
            {
                return RedirectToAction("VerifyIdentity");
            }

            // Load product details for each cart item
            foreach (var item in cart)
            {
                item.Product = await _productService.GetProductByIdAsync(item.ProductId);
            }

            ViewBag.CartItems = cart;
            ViewBag.CartTotal = cart.Sum(c => c.TotalPrice);
           // ViewBag.CustomerName = otpData.FullName;
            ViewBag.CustomerPhone = otpData.PhoneNumber;
           // ViewBag.CustomerEmail = otpData.Email;
            
            return View();
        }

        [HttpGet]
        public IActionResult VerifyIdentity()
        {
            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Store");
            }

            return View(new OtpVerificationModel());
        }

        [HttpPost]
        public async Task<IActionResult> VerifyIdentity(OtpVerificationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Generate and send OTP
                var otpCode = await _otpService.GenerateOtpAsync(model.PhoneNumber);
                var otpSent = await _otpService.SendOtpAsync(model.PhoneNumber,otpCode);

                if (!otpSent)
                {
                    TempData["Error"] = "Failed to send OTP. Please try again.";
                    return View(model);
                }

                // Store user data in session for OTP validation
                var otpSession = new OtpSession
                {
                  //  FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                 //   Email = model.Email,
                    OtpCode = otpCode,
                    ExpiryTime = DateTime.Now.AddMinutes(5),
                    IsVerified = false
                };

                HttpContext.Session.SetString("OtpSession", JsonSerializer.Serialize(otpSession));

                TempData["Success"] = $"OTP sent to {model.PhoneNumber}";
                return RedirectToAction("ValidateOtp");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending OTP: {ex.Message}");
                TempData["Error"] = "An error occurred while sending OTP. Please try again.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ValidateOtp()
        {
            var otpSessionJson = HttpContext.Session.GetString("OtpSession");
            if (string.IsNullOrEmpty(otpSessionJson))
            {
                return RedirectToAction("VerifyIdentity");
            }

            var otpSession = JsonSerializer.Deserialize<OtpSession>(otpSessionJson);
            
            var model = new OtpValidationModel
            {
               // FullName = otpSession.FullName,
                PhoneNumber = otpSession.PhoneNumber,
                //Email = otpSession.Email,
                ExpiryTime = otpSession.ExpiryTime
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ValidateOtp(OtpValidationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var otpSessionJson = HttpContext.Session.GetString("OtpSession");
            if (string.IsNullOrEmpty(otpSessionJson))
            {
                TempData["Error"] = "Session expired. Please start again.";
                return RedirectToAction("VerifyIdentity");
            }

            var otpSession = JsonSerializer.Deserialize<OtpSession>(otpSessionJson);

            // Check if OTP is expired
            if (DateTime.Now > otpSession.ExpiryTime)
            {
                TempData["Error"] = "OTP has expired. Please request a new one.";
                HttpContext.Session.Remove("OtpSession");
                return RedirectToAction("VerifyIdentity");
            }

            // Validate OTP
            if (_otpService.ValidateOtp(otpSession.PhoneNumber, model.OtpCode))
            {
                // Mark as verified
                otpSession.IsVerified = true;
                HttpContext.Session.SetString("OtpSession", JsonSerializer.Serialize(otpSession));

                TempData["Success"] = "Identity verified successfully!";
                return RedirectToAction("Checkout");
            }
            else
            {
                TempData["Error"] = "Invalid OTP. Please try again.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TestCart()
        {
            Console.WriteLine("TestCart GET method called");
            return View();
        }

        [HttpGet]
        public IActionResult TestForm()
        {
            Console.WriteLine("TestForm GET method called");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TestPlaceOrderSimple()
        {
            Console.WriteLine("TestPlaceOrderSimple GET method called");
            
            try
            {
                // Test with hardcoded values
                var result = await PlaceOrder(
                    "Test User", 
                    "test@example.com", 
                    "1234567890", 
                    "123 Test St", 
                    "Test City", 
                    "TS", 
                    "12345", 
                    "Bank Transfer"
                );
                
                Console.WriteLine("PlaceOrder method executed successfully");
                return Json(new { success = true, message = "Order placed successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TestPlaceOrderSimple: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult TestPlaceOrder()
        {
            Console.WriteLine("TestPlaceOrder GET method called");
            return Json(new { message = "PlaceOrder route is working", timestamp = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string customerName, string customerEmail, string customerPhone, 
            string shippingAddress, string city, string state, string zipCode, string paymentMethod)
        {
            // Debug logging
            Console.WriteLine($"PlaceOrder POST called with: {customerName}, {customerEmail}, {paymentMethod}");
            
            var cart = GetCartFromSession();
            Console.WriteLine($"Cart has {cart.Count} items");
            
            if (!cart.Any())
            {
                Console.WriteLine("Cart is empty, redirecting to store");
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Store");
            }

            try
            {
                // Create or get customer (for simplicity, we'll create a new customer record each time)
                var customer = new Customer
                {
                    FirstName = customerName.Split(' ').FirstOrDefault() ?? customerName,
                    LastName = customerName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    Email = customerEmail,
                    PhoneNumber = customerPhone,
                    Address = shippingAddress,
                    City = city,
                    State = state,
                    ZipCode = zipCode,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Load product details for cart items
                foreach (var item in cart)
                {
                    item.Product = await _productService.GetProductByIdAsync(item.ProductId);
                }

                // Calculate totals
                var subtotal = cart.Sum(c => c.Product.Price * c.Quantity);
                var tax = subtotal * 0.1m; // 10% tax
                var total = subtotal + tax;

                // Create order
                var order = new Order
                {
                    Customer = customer,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = total,
                    ShippingAddress = shippingAddress,
                    ShippingCity = city,
                    ShippingState = state,
                    ShippingZipCode = zipCode,
                    Notes = $"Payment Method: {paymentMethod}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    OrderItems = cart.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        UnitPrice = c.Product.Price,
                        CreatedAt = DateTime.UtcNow
                    }).ToList()
                };

                // Save order to database
                var savedOrder = await _orderService.CreateOrderAsync(order);

                // Clear cart after successful order
                HttpContext.Session.Remove("Cart");

                // Redirect to order confirmation
                TempData["Success"] = $"Order placed successfully! Order Number: {savedOrder.OrderNumber}";
                return RedirectToAction("OrderConfirmation", new { orderNumber = savedOrder.OrderNumber });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to place order. Please try again.";
                return RedirectToAction("Checkout");
            }
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            try
            {
                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                return cartItems ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        [HttpPost]
        public IActionResult StoreCustomerData([FromBody] CustomerData customerData)
        {
            try
            {
                var customerJson = JsonSerializer.Serialize(customerData);
                HttpContext.Session.SetString("CustomerData", customerJson);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> OrderConfirmation(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Load order with all related data
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

                if (order == null)
                {
                    TempData["Error"] = "Order not found";
                    return RedirectToAction("Index", "Home");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order confirmation for {OrderNumber}", orderNumber);
                TempData["Error"] = "An error occurred while loading your order confirmation.";
                return RedirectToAction("Index", "Home");
            }
        }
        
        public IActionResult TestOrderFlow()
        {
            return View();
        }


    }

    public class CustomerData
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
    public class RemoteCart
    {
        public int ProductId { get; set; }
    }
}
