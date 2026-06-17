using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstilosController : ControllerBase
    {
        private readonly WaybackContext _context;
        public EstilosController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstilosMenuDTO>>> GetEstilos()
        {
            var estilos = await _context.Estilos
                .OrderBy(e => e.EstNombre)
                .ToListAsync();

            if (!estilos.Any())
            {
                return Ok(new List<EstilosMenuDTO>());
            }

            var dto = estilos.Select(e => new EstilosMenuDTO
            {
                EstId = e.EstId,
                EstNombre = e.EstNombre
            }).ToList();

            return Ok(dto);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EstilosMenuDTO>> GetEstiloByID(int id)
        {
            var estilo = await _context.Estilos
                .FirstOrDefaultAsync(e => e.EstId == id);
            if (estilo == null)
            {
                return NotFound();
            }
            var dto = new EstilosMenuDTO
            {
                EstId = estilo.EstId,
                EstNombre = estilo.EstNombre
            };
            return Ok(dto);
        }
    }
}
