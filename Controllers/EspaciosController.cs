using Microsoft.AspNetCore.Mvc;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.ViewModels;

namespace DeportivoApp.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly IEspacioService _espacioService;

        public EspaciosController(IEspacioService espacioService)
        {
            _espacioService = espacioService;
        }

        // GET: Index
        public async Task<IActionResult> Index(string? tipo)
        {
            var espacios = await _espacioService.GetAllAsync();

            var tipos = espacios
                .Select(e => e.Tipo)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            ViewBag.Tipos = tipos;
            ViewBag.TipoSeleccionado = tipo;

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                espacios = espacios
                    .Where(e => string.Equals(e.Tipo, tipo, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(espacios);
        }

        // GET: Create
        public IActionResult Create()
        {
            var model = new EspacioCreateViewModel
            {
                Estado = "Disponible"
            };

            return View(model);
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(EspacioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var espacio = new Espacio
            {
                Nombre = model.Nombre,
                Tipo = model.Tipo,
                Capacidad = model.Capacidad,
                Estado = model.Estado
            };

            var result = await _espacioService.CreateAsync(espacio);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var espacio = await _espacioService.GetByIdAsync(id);
            if (espacio == null) return NotFound();

            var model = new EspacioCreateViewModel
            {
                Id = espacio.Id,
                Nombre = espacio.Nombre,
                Tipo = espacio.Tipo,
                Capacidad = espacio.Capacidad,
                Estado = espacio.Estado
            };

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EspacioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var espacio = await _espacioService.GetByIdAsync(model.Id);
            if (espacio == null) return NotFound();

            espacio.Nombre = model.Nombre;
            espacio.Tipo = model.Tipo;
            espacio.Capacidad = model.Capacidad;
            espacio.Estado = model.Estado;

            var result = await _espacioService.UpdateAsync(espacio);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var espacio = await _espacioService.GetByIdAsync(id);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Espacio espacio)
        {
            var ok = await _espacioService.DeleteAsync(espacio.Id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
