using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcHer.Data;
using MvcHer.Services;
using System;
using System.Threading.Tasks;

namespace MvcHer.Controllers
{
    public class OrderTrackingController : Controller
    {
        private readonly TeaShopDbContext _context;
        private readonly ISmsService _smsService;
        private readonly ILogger<OrderTrackingController> _logger;

        public OrderTrackingController(TeaShopDbContext context, ISmsService smsService, ILogger<OrderTrackingController> logger)
        {
            _context = context;
            _smsService = smsService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Track()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Track(string orderNumber, string phoneNumber)
        {
            if (string.IsNullOrEmpty(orderNumber) || string.IsNullOrEmpty(phoneNumber))
            {
                TempData["Error"] = "Please enter both order number and phone number.";
                return View();
            }

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

                if (order == null)
                {
                    TempData["Error"] = "Order not found. Please check your order number.";
                    return View();
                }

                // Verify phone number matches
                if (order.Customer?.PhoneNumber != phoneNumber)
                {
                    TempData["Error"] = "Phone number doesn't match our records.";
                    return View();
                }

                // Direct redirect to order details without OTP
                return RedirectToAction("OrderDetails", new { orderNumber = orderNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking order {OrderNumber}", orderNumber);
                TempData["Error"] = "An error occurred while tracking your order. Please try again.";
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> OrderDetails(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToAction("Track");
            }

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("Track");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details for {OrderNumber}", orderNumber);
                TempData["Error"] = "An error occurred while loading order details.";
                return RedirectToAction("Track");
            }
        }


    }
}
