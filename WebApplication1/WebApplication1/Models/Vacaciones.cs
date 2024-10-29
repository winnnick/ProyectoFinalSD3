using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Vacaciones
    {
        [Key]
        public int IdVacaciones { get; set; }

        [Required(ErrorMessage = "Falta el ID del empleado")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Falta la fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Falta la fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Falta el tipo de vacaciones")]
        public string TipoVacaciones { get; set; }

        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; }
    }
}
