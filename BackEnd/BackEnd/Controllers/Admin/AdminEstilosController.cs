using BackEnd.Data;
using BackEnd.DTOs.Admin;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/estilos")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminEstilosController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminEstilosController(WaybackContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AdminEstilosUpsertDTO>> CrearEstilo([FromBody] AdminEstilosUpsertDTO dto)
        {
            var nuevoEstilo = new Estilos
            {
                EstNombre = dto.EstNombre
            };

            _context.Estilos.Add(nuevoEstilo);
            await _context.SaveChangesAsync();

            return CreatedAtAction( //para ejecutar un metodo de otro controlador :
                actionName: "GetEstiloByID",//El nombre del metodo
                controllerName: "Estilos",  // sin el sufijo "Controller"
                routeValues: new { id = nuevoEstilo.EstId },
                value: new
                {
                    catId = nuevoEstilo.EstId,
                    catNombre = nuevoEstilo.EstNombre
                });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarEstilo(int id, [FromBody] AdminEstilosUpsertDTO dto)
        {
            var estilo = await _context.Estilos  
                .FirstOrDefaultAsync(e => e.EstId == id);

            if (estilo == null)
            {
                return NotFound($"El estilo con id: {id} no existe.");
            }

            estilo.EstNombre = dto.EstNombre;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarEstilo(int id)
        {
            var estilo = await _context.Estilos
                .FirstOrDefaultAsync(e => e.EstId == id);

            if (estilo == null) return NotFound($"EL estilo con id: {id} no existe.");

            var tieneProductos = await _context.Productos.AnyAsync(p => p.EstId == id);
            if (tieneProductos)
            {
                return Conflict("Este estilo tiene productos asociados, no se puede borrar.");
            }

            _context.Estilos.Remove(estilo); //intenta eliminar, si tiene productos asociados no se podra
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
