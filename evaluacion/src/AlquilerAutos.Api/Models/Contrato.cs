using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Api.Models;

public class Contrato
{
    public int Id { get; set; }

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; } = null!;

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }
    public EstadoContrato Estado { get; set; } = EstadoContrato.Activo;
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
