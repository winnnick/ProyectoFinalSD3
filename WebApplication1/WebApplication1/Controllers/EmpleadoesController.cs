using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly Recursos _context;

        public EmpleadosController(Recursos context)
        {
            _context = context;
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Empleado>>> Get()
        {
            return await _context.Empleados.ToListAsync();
        }

        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Empleado>> Get(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound("Empleado no encontrado.");
            }
            return Ok(empleado);
        }

        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return Ok("Empleado registrado correctamente.");
        }

        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Empleado empleado)
        {
            if (id != empleado.IdEmpleado)
            {
                return BadRequest("El ID del empleado no coincide.");
            }

            var existingEmpleado = await _context.Empleados.FindAsync(id);
            if (existingEmpleado == null)
            {
                return NotFound("Empleado no encontrado.");
            }

            // Actualizar propiedades
            existingEmpleado.Nombre = empleado.Nombre;
            existingEmpleado.Apellido = empleado.Apellido;
            existingEmpleado.FechaIngreso = empleado.FechaIngreso;
            existingEmpleado.Cargo = empleado.Cargo;
            // Agregar cualquier otra propiedad que deba actualizarse

            await _context.SaveChangesAsync();
            return Ok("Empleado actualizado correctamente.");
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound("Empleado no encontrado.");
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return Ok("Empleado eliminado correctamente.");
        }
    }
}
