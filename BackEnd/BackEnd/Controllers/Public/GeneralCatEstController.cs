using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api")]
    [ApiController]
    public class GeneralCatEstController : ControllerBase
    {
        private readonly WaybackContext _context;

        public GeneralCatEstController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriasDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.CatNombre)
                .ToListAsync();

            if (!categorias.Any())
            {
                return Ok(new List<CategoriasDTO>()); //regresa una lista vacia
                //para que sea controlado por el frontend con "No hay categorias disponibles" o similar
            }

            var dto = categorias.Select(c => new CategoriasDTO
            {
                CatID = c.CatId,
                CatNombre = c.CatNombre
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("categorias/{id:int}")]
        public async Task<ActionResult<CategoriasDTO>> GetCategoriaByID(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CatId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            var dto = new CategoriasDTO
            {
                CatID = categoria.CatId,
                CatNombre = categoria.CatNombre
            };
            return Ok(dto);
        }

        [HttpGet("estilos")]
        public async Task<ActionResult<IEnumerable<EstilosDTO>>> GetEstilos()
        {
            var estilos = await _context.Estilos
                .OrderBy(e => e.EstNombre)
                .ToListAsync();

            if (!estilos.Any())
            {
                return Ok(new List<EstilosDTO>());
            }

            var dto = estilos.Select(e => new EstilosDTO
            {
                EstId = e.EstId,
                EstNombre = e.EstNombre
            }).ToList();

            return Ok(dto);
        }
        [HttpGet("estilos/{id:int}")]
        public async Task<ActionResult<EstilosDTO>> GetEstiloByID(int id)
        {
            var estilo = await _context.Estilos
                .FirstOrDefaultAsync(e => e.EstId == id);
            if (estilo == null)
            {
                return NotFound();
            }
            var dto = new EstilosDTO
            {
                EstId = estilo.EstId,
                EstNombre = estilo.EstNombre
            };
            return Ok(dto);
        }
    }
}
