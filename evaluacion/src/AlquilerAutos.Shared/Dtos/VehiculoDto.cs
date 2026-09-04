using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Shared.Dtos;

public class VehiculoDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string? Color { get; set; }
    public decimal TarifaDiaria { get; set; }
    public EstadoVehiculo Estado { get; set; }
}

public class VehiculoCreateDto
{
    public string Placa { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string? Color { get; set; }
    public decimal TarifaDiaria { get; set; }
    public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Disponible;
}
