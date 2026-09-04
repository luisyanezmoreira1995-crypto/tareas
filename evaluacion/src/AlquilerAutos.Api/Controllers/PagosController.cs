using AlquilerAutos.Api.Data;
using AlquilerAutos.Api.Models;
using AlquilerAutos.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly AppDbContext _db;

    public PagosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PagoDto>>> GetAll()
    {
        var pagos = await _db.Pagos.AsNoTracking().ToListAsync();
        return Ok(pagos.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PagoDto>> GetById(int id)
    {
        var pago = await _db.Pagos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (pago is null) return NotFound();
        return Ok(ToDto(pago));
    }

    [HttpPost]
    public async Task<ActionResult<PagoDto>> Create(PagoCreateDto dto)
    {
        var contrato = await _db.Contratos.FirstOrDefaultAsync(c => c.Id == dto.ContratoId);
        if (contrato is null) return BadRequest("El contrato indicado no existe.");
        if (dto.Monto <= 0) return BadRequest("El monto debe ser mayor a cero.");

        var pago = new Pago
        {
            ContratoId = dto.ContratoId,
            FechaPago = dto.FechaPago,
            Monto = dto.Monto,
            MetodoPago = dto.MetodoPago,
            Referencia = dto.Referencia
        };

        _db.Pagos.Add(pago);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = pago.Id }, ToDto(pago));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pago = await _db.Pagos.FirstOrDefaultAsync(p => p.Id == id);
        if (pago is null) return NotFound();

        _db.Pagos.Remove(pago);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static PagoDto ToDto(Pago p) => new()
    {
        Id = p.Id,
        ContratoId = p.ContratoId,
        FechaPago = p.FechaPago,
        Monto = p.Monto,
        MetodoPago = p.MetodoPago,
        Referencia = p.Referencia
    };
}
