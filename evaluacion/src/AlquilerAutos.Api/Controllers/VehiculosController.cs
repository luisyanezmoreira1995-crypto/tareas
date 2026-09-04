using AlquilerAutos.Api.Data;
using AlquilerAutos.Api.Models;
using AlquilerAutos.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly AppDbContext _db;

    public VehiculosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetAll()
    {
        var vehiculos = await _db.Vehiculos.AsNoTracking().ToListAsync();
        return Ok(vehiculos.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehiculoDto>> GetById(int id)
    {
        var vehiculo = await _db.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        if (vehiculo is null) return NotFound();
        return Ok(ToDto(vehiculo));
    }

    [HttpPost]
    public async Task<ActionResult<VehiculoDto>> Create(VehiculoCreateDto dto)
    {
        var vehiculo = new Vehiculo
        {
            Placa = dto.Placa,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Anio = dto.Anio,
            Color = dto.Color,
            TarifaDiaria = dto.TarifaDiaria,
            Estado = dto.Estado
        };

        _db.Vehiculos.Add(vehiculo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = vehiculo.Id }, ToDto(vehiculo));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, VehiculoCreateDto dto)
    {
        var vehiculo = await _db.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);
        if (vehiculo is null) return NotFound();

        vehiculo.Placa = dto.Placa;
        vehiculo.Marca = dto.Marca;
        vehiculo.Modelo = dto.Modelo;
        vehiculo.Anio = dto.Anio;
        vehiculo.Color = dto.Color;
        vehiculo.TarifaDiaria = dto.TarifaDiaria;
        vehiculo.Estado = dto.Estado;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehiculo = await _db.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);
        if (vehiculo is null) return NotFound();

        var tieneContratos = await _db.Contratos.AnyAsync(c => c.VehiculoId == id);
        if (tieneContratos) return BadRequest("No se puede eliminar el vehículo porque tiene contratos registrados.");

        _db.Vehiculos.Remove(vehiculo);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static VehiculoDto ToDto(Vehiculo v) => new()
    {
        Id = v.Id,
        Placa = v.Placa,
        Marca = v.Marca,
        Modelo = v.Modelo,
        Anio = v.Anio,
        Color = v.Color,
        TarifaDiaria = v.TarifaDiaria,
        Estado = v.Estado
    };
}
