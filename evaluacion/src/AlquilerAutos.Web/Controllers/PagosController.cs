using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Shared.Enums;
using AlquilerAutos.Web.Models;
using AlquilerAutos.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAutos.Web.Controllers;

public class PagosController : Controller
{
    private readonly PagoApiClient _pagosApi;
    private readonly ContratoApiClient _contratosApi;

    public PagosController(PagoApiClient pagosApi, ContratoApiClient contratosApi)
    {
        _pagosApi = pagosApi;
        _contratosApi = contratosApi;
    }

    public async Task<IActionResult> Index()
    {
        var pagos = await _pagosApi.GetAllAsync();
        return View(pagos);
    }

    public async Task<IActionResult> Create(int? contratoId)
    {
        var vm = new PagoCreateViewModel
        {
            Contratos = await _contratosApi.GetAllAsync(),
            Pago = new PagoCreateDto
            {
                ContratoId = contratoId ?? 0,
                FechaPago = DateTime.Today,
                MetodoPago = MetodoPago.Efectivo
            }
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PagoCreateViewModel vm)
    {
        var (ok, error) = await _pagosApi.CreateAsync(vm.Pago);
        if (!ok)
        {
            ModelState.AddModelError(string.Empty, error ?? "No se pudo registrar el pago.");
            vm.Contratos = await _contratosApi.GetAllAsync();
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var pago = await _pagosApi.GetByIdAsync(id);
        if (pago is null) return NotFound();
        return View(pago);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _pagosApi.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
