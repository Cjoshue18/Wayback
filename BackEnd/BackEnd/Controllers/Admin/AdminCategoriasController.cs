using BackEnd.Data;
using BackEnd.DTOs.Admin;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/categorias")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminCategoriasController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminCategoriasController(WaybackContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AdminCategoriasUpsertDTO>> CrearCategoria([FromBody] AdminCategoriasUpsertDTO dto)
        {
            var nuevaCategoria = new Categorias
            {
                CatNombre = dto.CatNombre
            };

            _context.Categorias.Add(nuevaCategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction( //para ejecutar un metodo de otro controlador :
                actionName: "GetCategoriaByID",//El nombre del metodo
                controllerName: "Categorias",  // sin el sufijo "Controller"
                routeValues: new { id = nuevaCategoria.CatId },
                value: new
                {
                    catId = nuevaCategoria.CatId,
                    catNombre = nuevaCategoria.CatNombre
                });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarCategoria(int id, [FromBody] AdminCategoriasUpsertDTO dto)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CatId == id);

            if (categoria == null)
            {
                return NotFound($"La categoria con id: {id} no existe.");
            }

            categoria.CatNombre = dto.CatNombre;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CatId == id);

            if (categoria == null) return NotFound($"La categoría con id: {id} no existe.");

            var tieneProductos = await _context.Productos.AnyAsync(p => p.CatId == id);
            if (tieneProductos)
            {
                return Conflict("Esta categoria tiene productos asociados, no se puede borrar.");
            }

            _context.Categorias.Remove(categoria); //intenta eliminar, si tiene productos asociados no se podra
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
