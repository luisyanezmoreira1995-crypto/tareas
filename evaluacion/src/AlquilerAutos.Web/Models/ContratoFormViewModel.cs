using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Web.Models;

public class ContratoCreateViewModel
{
    public ContratoCreateDto Contrato { get; set; } = new();
    public List<ClienteDto> Clientes { get; set; } = new();
    public List<VehiculoDto> Vehiculos { get; set; } = new();
}

public class ContratoEditViewModel
{
    public int Id { get; set; }
    public ContratoUpdateDto Contrato { get; set; } = new();
    public string? ClienteNombre { get; set; }
    public string? VehiculoDescripcion { get; set; }
}
