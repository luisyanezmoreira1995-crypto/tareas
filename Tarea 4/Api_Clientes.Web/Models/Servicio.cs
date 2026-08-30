using System.ComponentModel.DataAnnotations;

namespace Api_Clientes.Web.Models
{
    public class Servicio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser positivo")]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a 0")]
        [Display(Name = "Duración (minutos)")]
        public int DuracionMinutos { get; set; }
    }
}
