using Microsoft.AspNetCore.Mvc;
using MvcHer.Data;
using MvcHer.Models;
using MvcHer.Services;
using System.Text.Json;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;

namespace MvcHer.Controllers
{
    public class PaymentController : Controller
    {
        private readonly TeaShopDbContext _context;
        private readonly IStripeService _stripeService;
        private readonly IOrderService _orderService;
        private readonly ISmsService _smsService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(TeaShopDbContext context, IStripeService stripeService, IOrderService orderService, ISmsService smsService, ILogger<PaymentController> logger)
        {
            _context = context;
            _stripeService = stripeService;
            _orderService = orderService;
            _smsService = smsService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutRequest request)
        {
            try
            {
                var successUrl = $"{Request.Scheme}://{Request.Host}/Payment/Success?session_id={{CHECKOUT_SESSION_ID}}";
                var cancelUrl = $"{Request.Scheme}://{Request.Host}/Cart/Checkout";

                var session = await _stripeService.CreateCheckoutSessionAsync(
                    request.Amount,
                    "usd",
                    successUrl,
                    cancelUrl
                );

                return Json(new
                {
                    success = true,
                    sessionId = session.Id,
                    checkoutUrl = session.Url
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Stripe checkout session");
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Success(string session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(session_id))
                {
                    return RedirectToAction("Checkout", "Cart");
                }

                // Get the Stripe session to verify payment
                var sessionService = new SessionService();
                var session = await sessionService.GetAsync(session_id);

                if (session.PaymentStatus == "paid")
                {
                    // Payment is successful, create the order in our system
                    var orderResult = await ProcessOrderAfterPayment(session);
                    
                    if (orderResult.Success)
                    {
                        // Clear the cart
                        HttpContext.Session.Remove("Cart");
                        
                        return RedirectToAction("OrderConfirmation", "Cart", new { orderNumber = orderResult.OrderNumber });
                    }
                    else
                    {
                        TempData["Error"] = "Payment successful but order creation failed. Please contact support.";
                        return RedirectToAction("Checkout", "Cart");
                    }
                }
                else
                {
                    TempData["Error"] = "Payment was not successful. Please try again.";
                    return RedirectToAction("Checkout", "Cart");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment success");
                TempData["Error"] = "An error occurred while processing your payment. Please contact support.";
                return RedirectToAction("Checkout", "Cart");
            }
        }

        private async Task<OrderResult> ProcessOrderAfterPayment(Session stripeSession)
        {
            try
            {
                // Get cart items from session
                var cartJson = HttpContext.Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return new OrderResult { Success = false, Error = "Cart is empty" };
                }

                var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
                if (!cart.Any())
                {
                    return new OrderResult { Success = false, Error = "Cart is empty" };
                }

                // Calculate total
                var total = cart.Sum(item => item.Product.Price * item.Quantity);

                // Get customer data from session (stored during checkout)
                var customerData = HttpContext.Session.GetString("CustomerData");
                if (string.IsNullOrEmpty(customerData))
                {
                    return new OrderResult { Success = false, Error = "Customer data not found" };
                }
                
                var customerInfo = JsonSerializer.Deserialize<CustomerInfo>(customerData);
                
                // Create customer
                var customer = new Customer
                {
                    FirstName = customerInfo.CustomerName.Split(' ').FirstOrDefault() ?? customerInfo.CustomerName,
                    LastName = customerInfo.CustomerName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    Email = customerInfo.CustomerEmail,
                    PhoneNumber = customerInfo.CustomerPhone,
                    Address = customerInfo.ShippingAddress,
                    City = customerInfo.City,
                    State = customerInfo.State,
                    ZipCode = customerInfo.ZipCode
                };

                // Create order
                var order = new Order
                {
                    OrderNumber = GenerateOrderNumber(),
                    Customer = customer,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = total,
                    Status = "Paid", // Since payment is verified
                    ShippingAddress = customerInfo.ShippingAddress,
                    ShippingCity = customerInfo.City,
                    ShippingState = customerInfo.State,
                    ShippingZipCode = customerInfo.ZipCode,
                    Notes = $"Stripe Session ID: {stripeSession.Id}, Payment Intent: {stripeSession.PaymentIntentId}",
                    OrderItems = cart.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    }).ToList()
                };

                // Save order to database
                await _orderService.CreateOrderAsync(order);

                // Send SMS confirmation
                try
                {
                    _logger.LogInformation("Attempting to send SMS for order {OrderNumber} to phone {Phone}", order.OrderNumber, customerInfo.CustomerPhone);
                    var trackingUrl = Url.Action("Track", "OrderTracking", null, Request.Scheme);
                    _logger.LogInformation("Tracking URL: {TrackingUrl}", trackingUrl);
                    
                    var smsSent = await _smsService.SendOrderConfirmationSmsAsync(customerInfo.CustomerPhone, order.OrderNumber, trackingUrl);
                    
                    if (smsSent)
                    {
                        _logger.LogInformation("Order confirmation SMS sent successfully for order {OrderNumber}", order.OrderNumber);
                    }
                    else
                    {
                        _logger.LogWarning("Order confirmation SMS failed to send for order {OrderNumber}", order.OrderNumber);
                    }
                }
                catch (Exception smsEx)
                {
                    _logger.LogError(smsEx, "Exception occurred while sending SMS for order {OrderNumber}", order.OrderNumber);
                    // Don't fail the order creation if SMS fails
                }

                // Clear cart
                HttpContext.Session.Remove("Cart");

                return new OrderResult { Success = true, OrderNumber = order.OrderNumber };
            }
            catch (Exception ex)
            {
                return new OrderResult { Success = false, Error = ex.Message };
            }
        }

        private string GenerateOrderNumber()
        {
            return $"ORD{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }
    }

    public class CreateCheckoutRequest
    {
        public decimal Amount { get; set; }
    }

    public class CustomerInfo
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class OrderResult
    {
        public bool Success { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
