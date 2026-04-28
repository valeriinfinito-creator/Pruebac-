using Microsoft.AspNetCore.Mvc;
using DeportivoApp.Services.Interfaces;

namespace DeportivoApp.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public async Task<IActionResult> Index()
        {
            var reportes = await _reporteService.GetReportesAsync();
            return View(reportes);
        }

        public async Task<IActionResult> EspacioTop()
        {
            var data = await _reporteService.GetEspacioConMasReservasAsync();
            return View(data);
        }

        public async Task<IActionResult> UsuariosTop()
        {
            var data = await _reporteService.GetUsuariosMasAtendidosAsync();
            return View(data);
        }

        public async Task<IActionResult> EspaciosTop()
        {
            var data = await _reporteService.GetEspaciosMasUsadosAsync();
            return View(data);
        }

        public async Task<IActionResult> TasaInasistencia()
        {
            var tasa = await _reporteService.GetTasaInasistenciaAsync();
            return View(tasa);
        }
    }
}