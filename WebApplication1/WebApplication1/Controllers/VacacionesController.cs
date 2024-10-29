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
    public class VacacionesController : ControllerBase
    {
        private readonly Recursos _context;

        public VacacionesController(Recursos context)
        {
            _context = context;
        }

        // Listar todos los registros de vacaciones
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Vacaciones>>> Get()
        {
            return await _context.vacaciones.Include(v => v.Empleado).ToListAsync();
        }

        // Obtener un registro de vacaciones por ID
        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Vacaciones>> Get(int id)
        {
            var vacaciones = await _context.vacaciones.Include(v => v.Empleado)
                                                      .FirstOrDefaultAsync(v => v.IdVacaciones == id);
            if (vacaciones == null)
            {
                return NotFound("Registro de vacaciones no encontrado.");
            }
            return Ok(vacaciones);
        }

        // Insertar un nuevo registro de vacaciones
        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Vacaciones vacaciones)
        {
            _context.vacaciones.Add(vacaciones);
            await _context.SaveChangesAsync();
            return Ok("Registro de vacaciones agregado correctamente.");
        }

        // Actualizar un registro de vacaciones existente
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Vacaciones vacaciones)
        {
            if (id != vacaciones.IdVacaciones)
            {
                return BadRequest("El ID del registro de vacaciones no coincide.");
            }

            var existingVacaciones = await _context.vacaciones.FindAsync(id);
            if (existingVacaciones == null)
            {
                return NotFound("Registro de vacaciones no encontrado.");
            }

            // Actualizar propiedades
            existingVacaciones.IdEmpleado = vacaciones.IdEmpleado;
            existingVacaciones.FechaInicio = vacaciones.FechaInicio;
            existingVacaciones.FechaFin = vacaciones.FechaFin;
            existingVacaciones.TipoVacaciones = vacaciones.TipoVacaciones;

            await _context.SaveChangesAsync();
            return Ok("Registro de vacaciones actualizado correctamente.");
        }

        // Eliminar un registro de vacaciones por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var vacaciones = await _context.vacaciones.FindAsync(id);
            if (vacaciones == null)
            {
                return NotFound("Registro de vacaciones no encontrado.");
            }

            _context.vacaciones.Remove(vacaciones);
            await _context.SaveChangesAsync();
            return Ok("Registro de vacaciones eliminado correctamente.");
        }
    }
}
