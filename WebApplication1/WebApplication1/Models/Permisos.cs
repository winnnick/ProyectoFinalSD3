using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Permisos
    {
        [Key]
        public int IdPermisos { get; set; } // Clave primaria

        [Required(ErrorMessage = "Falta el ID del empleado")]
        public int IdEmpleado { get; set; } // Clave foránea

        [Required(ErrorMessage = "Falta la fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } // Fecha de inicio del permiso

        [Required(ErrorMessage = "Falta la fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } // Fecha de fin del permiso

        [Required(ErrorMessage = "Falta el tipo de permiso")]
        public string TipoPermiso { get; set; } // Tipo de permiso (por ejemplo, personal, médico)

        // Navegación
        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; } // Relación con la entidad Empleado
    }
}
