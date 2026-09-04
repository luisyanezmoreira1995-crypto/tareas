using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlquilerAutos.Web.Controllers;

public class ClientesController : Controller
{
    private readonly ClienteApiClient _api;

    public ClientesController(ClienteApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _api.GetAllAsync();
        return View(clientes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _api.GetByIdAsync(id);
        if (cliente is null) return NotFound();
        return View(cliente);
    }

    public IActionResult Create() => View(new ClienteCreateDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        await _api.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _api.GetByIdAsync(id);
        if (cliente is null) return NotFound();

        var dto = new ClienteCreateDto
        {
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            DocumentoIdentidad = cliente.DocumentoIdentidad,
            Telefono = cliente.Telefono,
            Email = cliente.Email,
            Direccion = cliente.Direccion
        };
        ViewBag.Id = id;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClienteCreateDto dto)
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
        var cliente = await _api.GetByIdAsync(id);
        if (cliente is null) return NotFound();
        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var (ok, error) = await _api.DeleteAsync(id);
        if (!ok)
        {
            var cliente = await _api.GetByIdAsync(id);
            if (cliente is null) return NotFound();
            ViewBag.Error = error;
            return View(cliente);
        }

        return RedirectToAction(nameof(Index));
    }
}
