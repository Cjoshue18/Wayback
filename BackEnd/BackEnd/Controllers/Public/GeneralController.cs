using BackEnd.Data;
using BackEnd.DTOs.Public;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly WaybackContext _context;

        public GeneralController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .ToListAsync();

            var dto = categorias.Select(c => new CategoriasDTO
            {
                CatNombre = c.CatNombre
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("estilos")]
        public async Task<ActionResult<IEnumerable<Estilos>>> GetEstilos()
        {
            var estilos = await _context.Estilos
                .ToListAsync();

            var dto = estilos.Select(e => new EstilosDTO
            {
                EstNombre = e.EstNombre
            }).ToList();

            return Ok(dto);
        }
    }
}
