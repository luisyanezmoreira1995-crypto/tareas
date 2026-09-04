using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Shared.Dtos;

public class ReporteContratosClienteDto
{
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string DocumentoIdentidad { get; set; } = string.Empty;
    public List<ReporteContratoItemDto> Contratos { get; set; } = new();
}

public class ReporteContratoItemDto
{
    public int ContratoId { get; set; }
    public string VehiculoDescripcion { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoContrato Estado { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public List<PagoDto> Pagos { get; set; } = new();
}
