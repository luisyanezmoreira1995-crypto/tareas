using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Web.Models;
using AlquilerAutos.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAutos.Web.Controllers;

public class ContratosController : Controller
{
    private readonly ContratoApiClient _contratosApi;
    private readonly ClienteApiClient _clientesApi;
    private readonly VehiculoApiClient _vehiculosApi;

    public ContratosController(ContratoApiClient contratosApi, ClienteApiClient clientesApi, VehiculoApiClient vehiculosApi)
    {
        _contratosApi = contratosApi;
        _clientesApi = clientesApi;
        _vehiculosApi = vehiculosApi;
    }

    public async Task<IActionResult> Index()
    {
        var contratos = await _contratosApi.GetAllAsync();
        return View(contratos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var contrato = await _contratosApi.GetByIdAsync(id);
        if (contrato is null) return NotFound();
        return View(contrato);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new ContratoCreateViewModel
        {
            Clientes = await _clientesApi.GetAllAsync(),
            Vehiculos = (await _vehiculosApi.GetAllAsync())
                .Where(v => v.Estado == AlquilerAutos.Shared.Enums.EstadoVehiculo.Disponible)
                .ToList(),
            Contrato = new ContratoCreateDto { FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(1) }
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContratoCreateViewModel vm)
    {
        var (ok, error) = await _contratosApi.CreateAsync(vm.Contrato);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, error ?? "No se pudo crear el contrato.");
            vm.Clientes = await _clientesApi.GetAllAsync();
            vm.Vehiculos = (await _vehiculosApi.GetAllAsync())
                .Where(v => v.Estado == AlquilerAutos.Shared.Enums.EstadoVehiculo.Disponible)
                .ToList();
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var contrato = await _contratosApi.GetByIdAsync(id);
        if (contrato is null) return NotFound();

        var vm = new ContratoEditViewModel
        {
            Id = id,
            ClienteNombre = contrato.ClienteNombre,
            VehiculoDescripcion = contrato.VehiculoDescripcion,
            Contrato = new ContratoUpdateDto
            {
                FechaInicio = contrato.FechaInicio,
                FechaFin = contrato.FechaFin,
                FechaDevolucionReal = contrato.FechaDevolucionReal,
                Estado = contrato.Estado,
                Observaciones = contrato.Observaciones
            }
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContratoEditViewModel vm)
    {
        var (ok, error) = await _contratosApi.UpdateAsync(id, vm.Contrato);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, error ?? "No se pudo actualizar el contrato.");
            vm.Id = id;
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var contrato = await _contratosApi.GetByIdAsync(id);
        if (contrato is null) return NotFound();
        return View(contrato);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _contratosApi.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
