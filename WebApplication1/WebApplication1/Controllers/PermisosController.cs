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
    public class PermisosController : ControllerBase
    {
        private readonly Recursos _context;

        public PermisosController(Recursos context)
        {
            _context = context;
        }

        // Listar todos los permisos
        [HttpGet("Listar")]
        public async Task<ActionResult<IEnumerable<Permisos>>> Get()
        {
            return await _context.Permisos.Include(p => p.Empleado).ToListAsync();
        }

        // Obtener un permiso por ID
        [HttpGet("Listar/{id}")]
        public async Task<ActionResult<Permisos>> Get(int id)
        {
            var permiso = await _context.Permisos.Include(p => p.Empleado)
                                                 .FirstOrDefaultAsync(p => p.IdPermisos == id);
            if (permiso == null)
            {
                return NotFound("Permiso no encontrado.");
            }
            return Ok(permiso);
        }

        // Insertar un nuevo permiso
        [HttpPost("Insertar")]
        public async Task<ActionResult> Post(Permisos permiso)
        {
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();
            return Ok("Permiso registrado correctamente.");
        }

        // Actualizar un permiso existente
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> Put(int id, Permisos permiso)
        {
            if (id != permiso.IdPermisos)
            {
                return BadRequest("El ID del permiso no coincide.");
            }

            var existingPermiso = await _context.Permisos.FindAsync(id);
            if (existingPermiso == null)
            {
                return NotFound("Permiso no encontrado.");
            }

            // Actualizar propiedades
            existingPermiso.IdEmpleado = permiso.IdEmpleado;
            existingPermiso.FechaInicio = permiso.FechaInicio;
            existingPermiso.FechaFin = permiso.FechaFin;
            existingPermiso.TipoPermiso = permiso.TipoPermiso;

            await _context.SaveChangesAsync();
            return Ok("Permiso actualizado correctamente.");
        }

        // Eliminar un permiso por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound("Permiso no encontrado.");
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
            return Ok("Permiso eliminado correctamente.");
        }
    }
}
