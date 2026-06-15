using BackEnd.Data;
using BackEnd.DTOs.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly WaybackContext _context;

        public ProductosController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDescripcionDTO>> GetProductosByID(int id)
        {
            var dto = await _context.Productos
                .Where(p => p.ProId == id)
                .Select(p => new ProductoDescripcionDTO
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProGenero = p.ProGenero,
                    ProDescripcion = p.ProDescripcion != null ? p.ProDescripcion : "Sin descripción.",
                    ProPrecio = p.ProPrecio,
                    ProDescuentoInicio = p.ProDescuentoInicio,
                    ProDescuentoFin = p.ProDescuentoFin,
                    ImagenesUrl = p.Imagenes
                        .Select(i => i.ImgURL)
                        .ToList(),
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo != null ? p.Estilo.EstNombre : null,
                    Colores = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarColor.ColorHex)
                        .Distinct()
                        .ToList(),
                    Tallas = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarTalla)
                        .Distinct()
                        .ToList()
                })
                .FirstOrDefaultAsync();
            if (dto == null) return NotFound();

            return Ok(dto);
        }

        [HttpGet("categoria={id:int}")]
        public async Task<ActionResult<IEnumerable<ProductoTarjetaDTO>>> GetProductosByCategoriaID(int id)
        {
            var dto = await _context.Productos
                .Where(p => p.CatId == id)
                .OrderBy(p => p.ProId)
                .Select(p => new ProductoTarjetaDTO
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProPrecio = p.ProPrecio,
                    ProDescuento = p.ProDescuento,
                    ProDescuentoInicio = p.ProDescuentoInicio,
                    ProDescuentoFin = p.ProDescuentoFin,
                    ImagenesUrl = p.Imagenes
                        .Select(v => v.ImgURL)
                        .ToList(),
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo != null ? p.Estilo.EstNombre : null,
                    Colores = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarColor.ColorHex)
                        .Distinct()
                        .ToList(),
                    Tallas = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarTalla)
                        .Distinct()
                        .ToList()
                })
                .ToListAsync();
            if (!dto.Any())
            {
                return Ok(new List<ProductoTarjetaDTO>());
            }
            return Ok(dto);
        }

        [HttpGet("estilo={id:int}")]
        public async Task<ActionResult<IEnumerable<ProductoTarjetaDTO>>> GetProductosByEstiloID(int id)
        {
            var dto = await _context.Productos
                .Where(p => p.EstId == id)
                .OrderBy(p => p.ProId)
                .Select(p => new ProductoTarjetaDTO
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProPrecio = p.ProPrecio,
                    ProDescuento = p.ProDescuento,
                    ProDescuentoInicio = p.ProDescuentoInicio,
                    ProDescuentoFin = p.ProDescuentoFin,
                    ImagenesUrl = p.Imagenes
                        .Select(v => v.ImgURL)
                        .ToList(),
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo!.EstNombre, //como estoy buscando por estilo, no será nulo
                    Colores = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarColor.ColorHex)
                        .Distinct()
                        .ToList(),
                    Tallas = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarTalla)
                        .Distinct()
                        .ToList()
                })
                .ToListAsync();
            if (!dto.Any())
            {
                return Ok(new List<ProductoTarjetaDTO>());
            }
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoTarjetaDTO>>> GetProductoFilter([FromQuery] int? Categoria,
            [FromQuery] int? Estilo, [FromQuery] string? genero, [FromQuery] decimal? PrecioMin, [FromQuery] decimal? PrecioMax)
        {
            var query = _context.Productos.AsQueryable();
            if (Categoria.HasValue)
            {
                query = query.Where(p => p.CatId == Categoria.Value);
            }
            if (Estilo.HasValue)
            {
                query = query.Where(p => p.EstId == Estilo.Value);
            }
            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(p => p.ProGenero == genero);
            }
            if (PrecioMin.HasValue)
            {
                query = query.Where(p => p.ProPrecio >= PrecioMin.Value);
            }
            if (PrecioMax.HasValue)
            {
                query = query.Where(p => p.ProPrecio <= PrecioMax.Value);
            }

            var productos = await query
                .Select(p => new ProductoTarjetaDTO
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProPrecio = p.ProPrecio,
                    ProDescuento = p.ProDescuento,
                    ProDescuentoInicio = p.ProDescuentoInicio,
                    ProDescuentoFin = p.ProDescuentoFin,
                    ImagenesUrl = p.Imagenes
                        .Select(v => v.ImgURL)
                        .ToList(),
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo!.EstNombre, //como estoy buscando por estilo, no será nulo
                    Colores = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarColor.ColorHex)
                        .Distinct()
                        .ToList(),
                    Tallas = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .Select(v => v.VarTalla)
                        .Distinct()
                        .ToList()

                }).ToListAsync();

            if (!productos.Any())
            {
                return NotFound();
            }
            return Ok(productos);
        }
    }
}
