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
    public class CapacitacionesController : ControllerBase
    {
        private readonly Recursos _context;

        public CapacitacionesController(Recursos context)
        {
            _context = context;
        }

        // Listar todas las capacitaciones
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Capacitaciones>>> Get()
        {
            return await _context.capacitaciones.Include(c => c.Empleado).ToListAsync();
        }

        // Obtener una capacitación por ID
        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Capacitaciones>> Get(int id)
        {
            var capacitacion = await _context.capacitaciones.Include(c => c.Empleado)
                                                            .FirstOrDefaultAsync(c => c.IdCapacitaciones == id);
            if (capacitacion == null)
            {
                return NotFound("Capacitación no encontrada.");
            }
            return Ok(capacitacion);
        }

        // Insertar una nueva capacitación
        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Capacitaciones capacitacion)
        {
            _context.capacitaciones.Add(capacitacion);
            await _context.SaveChangesAsync();
            return Ok("Capacitación registrada correctamente.");
        }

        // Actualizar una capacitación existente
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Capacitaciones capacitacion)
        {
            if (id != capacitacion.IdCapacitaciones)
            {
                return BadRequest("El ID de la capacitación no coincide.");
            }

            var existingCapacitacion = await _context.capacitaciones.FindAsync(id);
            if (existingCapacitacion == null)
            {
                return NotFound("Capacitación no encontrada.");
            }

            // Actualizar propiedades
            existingCapacitacion.IdEmpleado = capacitacion.IdEmpleado;
            existingCapacitacion.FechaInicio = capacitacion.FechaInicio;
            existingCapacitacion.FechaFin = capacitacion.FechaFin;
            existingCapacitacion.TipoCapacitacion = capacitacion.TipoCapacitacion;

            await _context.SaveChangesAsync();
            return Ok("Capacitación actualizada correctamente.");
        }

        // Eliminar una capacitación por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var capacitacion = await _context.capacitaciones.FindAsync(id);
            if (capacitacion == null)
            {
                return NotFound("Capacitación no encontrada.");
            }

            _context.capacitaciones.Remove(capacitacion);
            await _context.SaveChangesAsync();
            return Ok("Capacitación eliminada correctamente.");
        }
    }
}
