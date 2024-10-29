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
    public class NominaController : ControllerBase
    {
        private readonly Recursos _context;

        public NominaController(Recursos context)
        {
            _context = context;
        }

        // Listar todos los registros de nómina
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Nomina>>> Get()
        {
            return await _context.Nominas.Include(n => n.Empleado).ToListAsync();
        }

        // Obtener un registro de nómina por ID
        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Nomina>> Get(int id)
        {
            var nomina = await _context.Nominas.Include(n => n.Empleado)
                                               .FirstOrDefaultAsync(n => n.IdNomina == id);
            if (nomina == null)
            {
                return NotFound("Registro de nómina no encontrado.");
            }
            return Ok(nomina);
        }

        // Insertar un nuevo registro de nómina
        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Nomina nomina)
        {
            _context.Nominas.Add(nomina);
            await _context.SaveChangesAsync();
            return Ok("Registro de nómina agregado correctamente.");
        }

        // Actualizar un registro de nómina existente
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Nomina nomina)
        {
            if (id != nomina.IdNomina)
            {
                return BadRequest("El ID del registro de nómina no coincide.");
            }

            var existingNomina = await _context.Nominas.FindAsync(id);
            if (existingNomina == null)
            {
                return NotFound("Registro de nómina no encontrado.");
            }

            // Actualizar propiedades
            existingNomina.IdEmpleado = nomina.IdEmpleado;
            existingNomina.FechaPago = nomina.FechaPago;
            existingNomina.MontoPago = nomina.MontoPago;

            await _context.SaveChangesAsync();
            return Ok("Registro de nómina actualizado correctamente.");
        }

        // Eliminar un registro de nómina por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var nomina = await _context.Nominas.FindAsync(id);
            if (nomina == null)
            {
                return NotFound("Registro de nómina no encontrado.");
            }

            _context.Nominas.Remove(nomina);
            await _context.SaveChangesAsync();
            return Ok("Registro de nómina eliminado correctamente.");
        }
    }
}
