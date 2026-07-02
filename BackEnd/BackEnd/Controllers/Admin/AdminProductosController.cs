using BackEnd.Data;
using BackEnd.DTOs;
using BackEnd.DTOs.Admin;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/productos")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminProductosController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminProductosController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ListaPaginada<AdminGetProductosDTO>>> GetListaProductos([FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            var query = _context.Productos.AsQueryable();
            var totalRegistros = await query.CountAsync();

            var time = DateTime.UtcNow;
            var productos = await query
                .OrderBy(p => p.ProId)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
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

            var result = new ListaPaginada<AdminGetProductosDTO>
            {
                TotalRegistros = totalRegistros,
                PaginaActual = pagina,
                RegistrosPorPagina = registrosPorPagina,
                Elementos = productos
            };

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdminGetProductosDTO>> GetProductoById(int id)
        {
            var time = DateTime.UtcNow;
            var dto = await _context.Productos
                .Where(p => p.ProId == id)
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

                }).FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<AdminUpsertProductosDTO>> CrearProducto([FromBody] AdminUpsertProductosDTO dto)
        {
            var nuevoProducto = new Productos
            {
                ProNombre = dto.ProNombre,
                ProDescripcion = dto.ProDescripcion,
                ProGenero = dto.ProGenero,
                CatId = dto.CatId,
                EstId = dto.EstId,
                ProPrecio = dto.ProPrecio,
                ProDescuento = dto.ProDescuento,
                ProDescuentoInicio = dto.ProDescuentoInicio,
                ProDescuentoFin = dto.ProDescuentoFin,
                ProFechaCreacion = DateTime.UtcNow
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductoById), new { id = nuevoProducto.ProId }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] AdminUpsertProductosDTO dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.ProId == id);

            if(producto == null)
            {
                return NotFound($"El producto con id: {id} no existe.");
            }

            producto.ProNombre = dto.ProNombre;
            producto.ProDescripcion = dto.ProDescripcion;
            producto.ProGenero = dto.ProGenero;
            producto.CatId = dto.CatId;
            producto.EstId = dto.EstId;
            producto.ProPrecio = dto.ProPrecio;
            producto.ProDescuento = dto.ProDescuento;
            producto.ProDescuentoInicio = dto.ProDescuentoInicio;
            producto.ProDescuentoFin = dto.ProDescuentoFin;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.ProId == id);

            if (producto == null) return NotFound($"El producto con id: {id} no existe.");

            _context.Productos.Remove(producto); //eliminación en cascada
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
