using AlquilerAutos.Shared.Enums;

namespace AlquilerAutos.Shared.Dtos;

public class PagoDto
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public string? Referencia { get; set; }
}

public class PagoCreateDto
{
    public int ContratoId { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public string? Referencia { get; set; }
}
