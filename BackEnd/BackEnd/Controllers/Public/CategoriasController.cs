using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly WaybackContext _context;

        public CategoriasController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriasMenuDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.CatNombre)
                .ToListAsync();

            if (!categorias.Any())
            {
                return Ok(new List<CategoriasMenuDTO>()); //regresa una lista vacia
                //para que sea controlado por el frontend con "No hay categorias disponibles" o similar
            }

            var dto = categorias.Select(c => new CategoriasMenuDTO
            {
                CatID = c.CatId,
                CatNombre = c.CatNombre
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriasMenuDTO>> GetCategoriaByID(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CatId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            var dto = new CategoriasMenuDTO
            {
                CatID = categoria.CatId,
                CatNombre = categoria.CatNombre
            };
            return Ok(dto);
        }
    }
}
