using DeportivoApp.Models;
using DeportivoApp.Services.Interfaces;
using DeportivoApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeportivoApp.Controllers;

public class ReservasController : Controller
{
    private readonly IReservaService _reservaService;
    private readonly IEspacioService _espacioService;
    private readonly IUsuarioService _usuarioService;

    public ReservasController(
        IReservaService reservaService,
        IEspacioService espacioService,
        IUsuarioService usuarioService
    )
    {
        _reservaService = reservaService;
        _espacioService = espacioService;
        _usuarioService = usuarioService;
    }

    // INDEX
    public async Task<IActionResult> Index(int? usuarioId, int? espacioId)
    {
        var reservas = await _reservaService.GetAllAsync();

        var usuarios = await _usuarioService.GetAllAsync();
        var espacios = await _espacioService.GetAllAsync();

        ViewBag.Usuarios = usuarios
            .Select(x => new SelectListItem(x.NombreCompleto, x.Id.ToString(), usuarioId == x.Id))
            .ToList();

        ViewBag.Espacios = espacios
            .Select(x => new SelectListItem(x.Nombre, x.Id.ToString(), espacioId == x.Id))
            .ToList();

        if (usuarioId.HasValue)
            reservas = reservas.Where(r => r.UsuarioId == usuarioId.Value).ToList();

        if (espacioId.HasValue)
            reservas = reservas.Where(r => r.EspacioId == espacioId.Value).ToList();

        return View(reservas);
    }

    // GET CREATE
    public async Task<IActionResult> Create()
    {
        var vm = await ConstruirViewModelAsync(new ReservaCreateViewModel());
        return View(vm);
    }

    // POST CREATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservaCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await ConstruirViewModelAsync(vm);
            return View(vm);
        }

        var result = await _reservaService.CreateAsync(new Reserva
        {
            EspacioId = vm.EspacioId,
            UsuarioId = vm.UsuarioId,
            Fecha = vm.Fecha,
            HoraInicio = vm.HoraInicio,
            HoraFin = vm.HoraFin
        });

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            vm = await ConstruirViewModelAsync(vm);
            return View(vm);
        }

        TempData["Ok"] = "Reserva creada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // GET EDIT
    public async Task<IActionResult> Edit(int id)
    {
        var reserva = await _reservaService.GetByIdAsync(id);
        if (reserva == null) return NotFound();

        var vm = new ReservaCreateViewModel
        {
            Id = reserva.Id,
            UsuarioId = reserva.UsuarioId,
            EspacioId = reserva.EspacioId,
            Fecha = reserva.Fecha,
            HoraInicio = reserva.HoraInicio,
            HoraFin = reserva.HoraFin
        };

        vm = await ConstruirViewModelAsync(vm);
        return View(vm);
    }

    // POST EDIT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReservaCreateViewModel vm)
    {
        if (id != vm.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            vm = await ConstruirViewModelAsync(vm);
            return View(vm);
        }

        var result = await _reservaService.UpdateAsync(new Reserva
        {
            Id = vm.Id,
            UsuarioId = vm.UsuarioId,
            EspacioId = vm.EspacioId,
            Fecha = vm.Fecha,
            HoraInicio = vm.HoraInicio,
            HoraFin = vm.HoraFin
        });

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            vm = await ConstruirViewModelAsync(vm);
            return View(vm);
        }

        TempData["Ok"] = "Reserva actualizada correctamente";
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var reserva = await _reservaService.GetByIdAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Reserva reserva)
    {
        var ok = await _reservaService.DeleteAsync(reserva.Id);
        if (!ok) return NotFound();

        TempData["Ok"] = "Reserva eliminada";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var ok = await _reservaService.CambiarEstadoAsync(id, "Cancelada");
        if (!ok) return NotFound();
        TempData["Ok"] = "Reserva cancelada";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(int id)
    {
        var ok = await _reservaService.CambiarEstadoAsync(id, "Finalizada");
        if (!ok) return NotFound();
        TempData["Ok"] = "Reserva finalizada";
        return RedirectToAction(nameof(Index));
    }

    // CONSTRUIR VIEWMODEL
    private async Task<ReservaCreateViewModel> ConstruirViewModelAsync(ReservaCreateViewModel vm)
    {
        var espacios = await _espacioService.GetAllAsync();
        var usuarios = await _usuarioService.GetAllAsync();

        vm.Espacios = espacios
            .Select(x => new SelectListItem(x.Nombre, x.Id.ToString()))
            .ToList();

        vm.Usuarios = usuarios
            .Select(x => new SelectListItem(x.NombreCompleto, x.Id.ToString()))
            .ToList();

        return vm;
    }
}
