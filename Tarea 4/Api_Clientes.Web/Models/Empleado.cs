using System.ComponentModel.DataAnnotations;

namespace Api_Clientes.Web.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo es obligatorio")]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser positivo")]
        [Display(Name = "Salario")]
        [DataType(DataType.Currency)]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        [Display(Name = "Fecha de contratación")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; } = DateTime.Today;
    }
}
