using AlquilerAutos.Api.Data;
using AlquilerAutos.Api.Models;
using AlquilerAutos.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        var clientes = await _db.Clientes.AsNoTracking().ToListAsync();
        return Ok(clientes.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound();
        return Ok(ToDto(cliente));
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteCreateDto dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            DocumentoIdentidad = dto.DocumentoIdentidad,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            FechaRegistro = DateTime.UtcNow
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, ToDto(cliente));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClienteCreateDto dto)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound();

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.DocumentoIdentidad = dto.DocumentoIdentidad;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;
        cliente.Direccion = dto.Direccion;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound();

        var tieneContratos = await _db.Contratos.AnyAsync(c => c.ClienteId == id);
        if (tieneContratos) return BadRequest("No se puede eliminar el cliente porque tiene contratos registrados.");

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ClienteDto ToDto(Cliente c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Apellido = c.Apellido,
        DocumentoIdentidad = c.DocumentoIdentidad,
        Telefono = c.Telefono,
        Email = c.Email,
        Direccion = c.Direccion,
        FechaRegistro = c.FechaRegistro
    };
}
