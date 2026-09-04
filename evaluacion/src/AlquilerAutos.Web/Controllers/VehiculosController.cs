using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAutos.Web.Controllers;

public class VehiculosController : Controller
{
    private readonly VehiculoApiClient _api;

    public VehiculosController(VehiculoApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var vehiculos = await _api.GetAllAsync();
        return View(vehiculos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var vehiculo = await _api.GetByIdAsync(id);
        if (vehiculo is null) return NotFound();
        return View(vehiculo);
    }

    public IActionResult Create() => View(new VehiculoCreateDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehiculoCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        await _api.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vehiculo = await _api.GetByIdAsync(id);
        if (vehiculo is null) return NotFound();

        var dto = new VehiculoCreateDto
        {
            Placa = vehiculo.Placa,
            Marca = vehiculo.Marca,
            Modelo = vehiculo.Modelo,
            Anio = vehiculo.Anio,
            Color = vehiculo.Color,
            TarifaDiaria = vehiculo.TarifaDiaria,
            Estado = vehiculo.Estado
        };
        ViewBag.Id = id;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VehiculoCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            return View(dto);
        }

        await _api.UpdateAsync(id, dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var vehiculo = await _api.GetByIdAsync(id);
        if (vehiculo is null) return NotFound();
        return View(vehiculo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, error) = await _api.DeleteAsync(id);
        if (!ok)
        {
            var vehiculo = await _api.GetByIdAsync(id);
            if (vehiculo is null) return NotFound();
            ViewBag.Error = error;
            return View(vehiculo);
        }

        return RedirectToAction(nameof(Index));
    }
}
