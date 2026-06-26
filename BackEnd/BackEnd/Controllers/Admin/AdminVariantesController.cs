using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminVariantesController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminVariantesController(WaybackContext context)
        {
            _context = context;
        }
        [HttpGet("productos/{ProId:int}/variantes")]
        public async Task<ActionResult<IEnumerable<VariantesDetalleDTO>>> GetVariantesByProductoId(int ProId)
        {
            var dto = await _context.Variantes
                .Where(v => v.ProId == ProId)
                .OrderBy(v => v.VarId)
                .Select(v => new VariantesDetalleDTO
                {
                    VarId = v.VarId,
                    VarTalla = v.VarTalla,
                    ColorHex = v.VarColor.ColorHex,
                    ColorNombre = v.VarColor.ColorNombre,
                    VarStock = v.VarStock
                }).ToListAsync();

            if (!dto.Any())
            {
                return Ok(new List<VariantesDetalleDTO>());
            }
            return Ok(dto);
        }

        [HttpGet("productos/{ProId:int}/variantes/{VarId:int}")]
        public async Task<ActionResult<VariantesDetalleDTO>> GetVarianteByVarId(int ProId, int VarId)
        {
            var dto = await _context.Variantes
                .Where(v => v.ProId == ProId && v.VarId == VarId)
                .Select(v => new VariantesDetalleDTO
                {
                    VarId = v.VarId,
                    VarTalla = v.VarTalla,
                    ColorHex = v.VarColor.ColorHex,
                    ColorNombre = v.VarColor.ColorNombre,
                    VarStock = v.VarStock
                }).FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }
    }
}
