using AlquilerAutos.Api.Data;
using AlquilerAutos.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlquilerAutos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("contratos-por-cliente/{clienteId:int}")]
    public async Task<ActionResult<ReporteContratosClienteDto>> ContratosPorCliente(int clienteId)
    {
        var cliente = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == clienteId);
        if (cliente is null) return NotFound();

        var contratos = await _db.Contratos
            .AsNoTracking()
            .Include(c => c.Vehiculo)
            .Include(c => c.Pagos)
            .Where(c => c.ClienteId == clienteId)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();

        var reporte = new ReporteContratosClienteDto
        {
            ClienteId = cliente.Id,
            ClienteNombre = $"{cliente.Nombre} {cliente.Apellido}",
            DocumentoIdentidad = cliente.DocumentoIdentidad,
            Contratos = contratos.Select(c =>
            {
                var totalPagado = c.Pagos.Sum(p => p.Monto);
                return new ReporteContratoItemDto
                {
                    ContratoId = c.Id,
                    VehiculoDescripcion = $"{c.Vehiculo.Marca} {c.Vehiculo.Modelo} ({c.Vehiculo.Placa})",
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    Estado = c.Estado,
                    Total = c.Total,
                    TotalPagado = totalPagado,
                    SaldoPendiente = c.Total - totalPagado,
                    Pagos = c.Pagos.Select(p => new PagoDto
                    {
                        Id = p.Id,
                        ContratoId = p.ContratoId,
                        FechaPago = p.FechaPago,
                        Monto = p.Monto,
                        MetodoPago = p.MetodoPago,
                        Referencia = p.Referencia
                    }).OrderBy(p => p.FechaPago).ToList()
                };
            }).ToList()
        };

        return Ok(reporte);
    }
}
