using BackEnd.Data;
using BackEnd.DTOs.Admin;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/productos")]
    [ApiController]
    public class AdminProductosController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminProductosController(WaybackContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminGetProductosDTO>>> GetListaProductos()
        {
            var time = DateTime.UtcNow;
            var dto = await _context.Productos
                .OrderBy(p => p.ProId)
                .Select(p => new AdminGetProductosDTO 
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProGenero = p.ProGenero,
                    ProCategoria = p.Categoria.CatNombre,
                    ProEstilo = p.Estilo != null ? p.Estilo.EstNombre : null,
                    ProPrecio = p.ProPrecio,
                    ProDescuento = p.ProDescuento,
                    ProVigenteDescuento = p.ProDescuento.HasValue && p.ProDescuentoInicio <= time &&
                        p.ProDescuentoFin >= time,
                    ProFechaCreacion = p.ProFechaCreacion,
                    ProTotalStock = p.Variantes.Sum(v => v.VarStock),
                
                }).ToListAsync();

            if (!dto.Any())
            {
                return Ok(new List<AdminGetProductosDTO>());
            }
            return Ok(dto);
        }
    }
}
