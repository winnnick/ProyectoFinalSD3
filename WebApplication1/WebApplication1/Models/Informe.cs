using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Informes
    {
        [Key]
        public int IdInformes { get; set; }

        [Required(ErrorMessage = "Falta el ID del empleado")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Falta la fecha de generación")]
        [DataType(DataType.Date)]
        public DateTime FechaGeneracion { get; set; }

        [Required(ErrorMessage = "Falta el contenido del informe")]
        public string ContenidoInforme { get; set; }

        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; }
    }
}
