using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Api.Models;

public class Vehiculo
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string? Color { get; set; }
    public decimal TarifaDiaria { get; set; }
    public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Disponible;

    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
