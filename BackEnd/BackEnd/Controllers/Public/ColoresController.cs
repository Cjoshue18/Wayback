using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColoresController : ControllerBase
    {
        private readonly WaybackContext _context;
        public ColoresController(WaybackContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColoresMenuDTO>>> GetColores()
        {
            var colores = await _context.VarColores
                .ToListAsync();
            if (!colores.Any())
            {
                return Ok(new List<ColoresMenuDTO>());
            }
            var dto = colores.Select(c => new ColoresMenuDTO
            {
                ColorId = c.ColorId,
                ColorHex = c.ColorHex,
            }).ToList();
            return Ok(dto);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ColoresMenuDTO>> GetColorById(int id)
        {
            var color = await _context.VarColores
                .FirstOrDefaultAsync(c => c.ColorId == id);
            if (color == null)
            {
                return NotFound();
            }
            var dto = new ColoresMenuDTO
            {
                ColorId = color.ColorId,
                ColorHex = color.ColorHex
            };
            return Ok(dto);
        }
    }
}
