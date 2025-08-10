using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcHer.Services;
using MvcHer.Models;

namespace MvcHer.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Track(string orderNumber = null)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return View();
            }

            try
            {
                var order = await _orderService.GetOrderByNumberAsync(orderNumber);
                
                if (order == null)
                {
                    TempData["Error"] = "Order not found. Please check your order number and try again.";
                    return View();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while tracking your order. Please try again.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                
                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("Track");
                }

                return View("Track", order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving order details.";
                return RedirectToAction("Track");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found" });
                }

                order.Status = status;
                order.UpdatedAt = DateTime.UtcNow;
                
                await _orderService.UpdateOrderAsync(order);
                
                return Json(new { success = true, message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to update order status" });
            }
        }
    }
}
