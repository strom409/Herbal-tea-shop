using Microsoft.AspNetCore.Mvc;
using MvcHer.Data;
using MvcHer.Services;

namespace MvcHer.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAboutUsService _aboutUsService;
        public AboutController(ILogger<HomeController> logger, IAboutUsService aboutUsService)
        {
            _logger = logger;
            _aboutUsService = aboutUsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> About()
        {
            var aboutUs = await _aboutUsService.GetAboutUsAsync();
            return View(aboutUs);
        }
    }
}
