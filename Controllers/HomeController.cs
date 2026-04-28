using Microsoft.AspNetCore.Mvc;
using DeportivoApp.Services.Interfaces;

namespace DeportivoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReporteService _reporteService;

        public HomeController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        // Dashboard principal
        public async Task<IActionResult> Index()
        {
            var dashboard = await _reporteService.GetDashboardAsync();
            return View(dashboard);
        }

        // Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}