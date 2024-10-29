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
    public class InformesController : ControllerBase
    {
        private readonly Recursos _context;

        public InformesController(Recursos context)
        {
            _context = context;
        }

        // Listar todos los informes
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Informes>>> Get()
        {
            return await _context.informes.Include(i => i.Empleado).ToListAsync();
        }

        // Obtener un informe por ID
        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Informes>> Get(int id)
        {
            var informe = await _context.informes.Include(i => i.Empleado)
                                                  .FirstOrDefaultAsync(i => i.IdInformes == id);
            if (informe == null)
            {
                return NotFound("Informe no encontrado.");
            }
            return Ok(informe);
        }

        // Insertar un nuevo informe
        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Informes informe)
        {
            _context.informes.Add(informe);
            await _context.SaveChangesAsync();
            return Ok("Informe registrado correctamente.");
        }

        // Actualizar un informe existente
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Informes informe)
        {
            if (id != informe.IdInformes)
            {
                return BadRequest("El ID del informe no coincide.");
            }

            var existingInforme = await _context.informes.FindAsync(id);
            if (existingInforme == null)
            {
                return NotFound("Informe no encontrado.");
            }

            // Actualizar propiedades
            existingInforme.IdEmpleado = informe.IdEmpleado;
            existingInforme.FechaGeneracion = informe.FechaGeneracion;
            existingInforme.ContenidoInforme = informe.ContenidoInforme;

            await _context.SaveChangesAsync();
            return Ok("Informe actualizado correctamente.");
        }

        // Eliminar un informe por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var informe = await _context.informes.FindAsync(id);
            if (informe == null)
            {
                return NotFound("Informe no encontrado.");
            }

            _context.informes.Remove(informe);
            await _context.SaveChangesAsync();
            return Ok("Informe eliminado correctamente.");
        }
    }
}
