using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Empleado
{
    [Key]
    public int IdEmpleado { get; set; }

    [Required(ErrorMessage = "Falta el nombre")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Falta el apellido")]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "Falta la fecha de ingreso")]
    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; }

    [Required(ErrorMessage = "Falta el cargo")]
    public string Cargo { get; set; }
}