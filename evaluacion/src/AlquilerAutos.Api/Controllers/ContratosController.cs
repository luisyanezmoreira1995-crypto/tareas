using AlquilerAutos.Api.Data;
using AlquilerAutos.Api.Models;
using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratosController : ControllerBase
{
    private readonly AppDbContext _db;

    public ContratosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContratoDto>>> GetAll()
    {
        var contratos = await _db.Contratos
            .AsNoTracking()
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .ToListAsync();

        return Ok(contratos.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContratoDto>> GetById(int id)
    {
        var contrato = await _db.Contratos
            .AsNoTracking()
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato is null) return NotFound();
        return Ok(ToDto(contrato));
    }

    [HttpPost]
    public async Task<ActionResult<ContratoDto>> Create(ContratoCreateDto dto)
    {
        var vehiculo = await _db.Vehiculos.FirstOrDefaultAsync(v => v.Id == dto.VehiculoId);
        if (vehiculo is null) return BadRequest("El vehículo indicado no existe.");
        if (vehiculo.Estado != EstadoVehiculo.Disponible) return BadRequest("El vehículo no está disponible.");

        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == dto.ClienteId);
        if (cliente is null) return BadRequest("El cliente indicado no existe.");

        if (dto.FechaFin < dto.FechaInicio) return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var dias = Math.Max(1, (dto.FechaFin.Date - dto.FechaInicio.Date).Days);
        var total = dias * vehiculo.TarifaDiaria;

        var contrato = new Contrato
        {
            ClienteId = dto.ClienteId,
            VehiculoId = dto.VehiculoId,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Estado = EstadoContrato.Activo,
            Total = total,
            Observaciones = dto.Observaciones
        };

        vehiculo.Estado = EstadoVehiculo.Alquilado;

        _db.Contratos.Add(contrato);
        await _db.SaveChangesAsync();

        contrato.Cliente = cliente;
        contrato.Vehiculo = vehiculo;

        return CreatedAtAction(nameof(GetById), new { id = contrato.Id }, ToDto(contrato));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ContratoUpdateDto dto)
    {
        var contrato = await _db.Contratos
            .Include(c => c.Vehiculo)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato is null) return NotFound();
        if (dto.FechaFin < dto.FechaInicio) return BadRequest("La fecha de fin no puede ser anterior a la fecha de inicio.");

        var estadoAnterior = contrato.Estado;

        contrato.FechaInicio = dto.FechaInicio;
        contrato.FechaFin = dto.FechaFin;
        contrato.FechaDevolucionReal = dto.FechaDevolucionReal;
        contrato.Estado = dto.Estado;
        contrato.Observaciones = dto.Observaciones;

        var dias = Math.Max(1, (dto.FechaFin.Date - dto.FechaInicio.Date).Days);
        contrato.Total = dias * contrato.Vehiculo.TarifaDiaria;

        if (estadoAnterior == EstadoContrato.Activo && dto.Estado != EstadoContrato.Activo)
        {
            contrato.Vehiculo.Estado = EstadoVehiculo.Disponible;
        }
        else if (estadoAnterior != EstadoContrato.Activo && dto.Estado == EstadoContrato.Activo)
        {
            contrato.Vehiculo.Estado = EstadoVehiculo.Alquilado;
        }

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contrato = await _db.Contratos.Include(c => c.Vehiculo).FirstOrDefaultAsync(c => c.Id == id);
        if (contrato is null) return NotFound();

        if (contrato.Estado == EstadoContrato.Activo)
        {
            contrato.Vehiculo.Estado = EstadoVehiculo.Disponible;
        }

        _db.Contratos.Remove(contrato);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ContratoDto ToDto(Contrato c) => new()
    {
        Id = c.Id,
        ClienteId = c.ClienteId,
        ClienteNombre = c.Cliente is not null ? $"{c.Cliente.Nombre} {c.Cliente.Apellido}" : null,
        VehiculoId = c.VehiculoId,
        VehiculoDescripcion = c.Vehiculo is not null ? $"{c.Vehiculo.Marca} {c.Vehiculo.Modelo} ({c.Vehiculo.Placa})" : null,
        FechaInicio = c.FechaInicio,
        FechaFin = c.FechaFin,
        FechaDevolucionReal = c.FechaDevolucionReal,
        Estado = c.Estado,
        Total = c.Total,
        Observaciones = c.Observaciones
    };
}
