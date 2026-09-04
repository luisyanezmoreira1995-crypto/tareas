using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Models;

public class ReporteViewModel
{
    public List<ClienteDto> Clientes { get; set; } = new();
    public int? ClienteSeleccionadoId { get; set; }
    public ReporteContratosClienteDto? Reporte { get; set; }
}
