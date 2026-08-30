namespace Api_Clientes.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaContratacion { get; set; }
    }
}
