using AlquilerAutos.Shared.Dtos;

namespace AlquilerAutos.Web.Models;

public class PagoCreateViewModel
{
    public PagoCreateDto Pago { get; set; } = new();
    public List<ContratoDto> Contratos { get; set; } = new();
}
