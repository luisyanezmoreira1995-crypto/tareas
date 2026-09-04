using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Api.Models;

public class Pago
{
    public int Id { get; set; }

    public int ContratoId { get; set; }
    public Contrato Contrato { get; set; } = null!;

    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public string? Referencia { get; set; }
}
