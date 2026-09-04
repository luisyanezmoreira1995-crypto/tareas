using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Shared.Dtos;

public class ContratoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int VehiculoId { get; set; }
    public string? VehiculoDescripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }
    public EstadoContrato Estado { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
}

public class ContratoCreateDto
{
    public int ClienteId { get; set; }
    public int VehiculoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? Observaciones { get; set; }
}

public class ContratoUpdateDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }
    public EstadoContrato Estado { get; set; }
    public string? Observaciones { get; set; }
}
