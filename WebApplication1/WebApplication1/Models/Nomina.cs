using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Nomina
    {
        [Key]
        public int IdNomina { get; set; }

        [Required(ErrorMessage = "Falta el ID del empleado")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Falta la fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "Falta el monto de pago")]
        public decimal MontoPago { get; set; }

        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; }
    }
}
