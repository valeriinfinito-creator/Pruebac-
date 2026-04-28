using Microsoft.AspNetCore.Mvc;
using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;

namespace DeportivoApp.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly IEspacioService _espacioService;

        public EspaciosController(IEspacioService espacioService)
        {
            _espacioService = espacioService;
        }

        // GET: Espacios
        public async Task<IActionResult> Index()
        {
            var espacios = await _espacioService.GetAllAsync();
            return View(espacios);
        }

        // GET: Espacios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var espacio = await _espacioService.GetByIdAsync(id);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        // GET: Espacios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Espacios/Create
        [HttpPost]
        public async Task<IActionResult> Create(Espacio espacio)
        {
            if (!ModelState.IsValid)
                return View(espacio);

            var result = await _espacioService.CreateAsync(espacio);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(espacio);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Espacios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var espacio = await _espacioService.GetByIdAsync(id);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        // POST: Espacios/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Espacio espacio)
        {
            if (!ModelState.IsValid)
                return View(espacio);

            var result = await _espacioService.UpdateAsync(espacio);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(espacio);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Espacios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var espacio = await _espacioService.GetByIdAsync(id);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        // POST: Espacios/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _espacioService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}