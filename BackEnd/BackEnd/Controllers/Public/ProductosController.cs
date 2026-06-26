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
                    Variantes = p.Variantes.Select(v => new VariantesDetalleDTO
                    {
                        VarId = v.VarId,
                        ColorNombre = v.VarColor.ColorNombre,
                        ColorHex = v.VarColor.ColorHex,
                        VarTalla = v.VarTalla,
                        VarStock = v.VarStock
                    }).ToList()
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
        public async Task<ActionResult<IEnumerable<ProductoTarjetaDTO>>> GetProductoFilter([FromQuery] List<int>? Categoria,
            [FromQuery] List<int>? Estilo, [FromQuery] string? genero, [FromQuery] decimal? PrecioMin, [FromQuery] decimal? PrecioMax,
            [FromQuery] List<string>? color, [FromQuery] List<string>? talla, [FromQuery] bool? stock) 
            //usamos filtros para poder filtrar por mas filtros del mismo tipo
        {
            var query = _context.Productos.AsQueryable();
            if (Categoria != null && Categoria.Any())
            {
                query = query.Where(p => Categoria.Contains(p.CatId)); //dame los productos cuyo catid este dentro de la lista
                //algo como: Select * FROM Productos WHERE cat_id IN (List<int> categorias)
            }
            if (Estilo != null && Estilo.Any())
            {
                query = query.Where(p => p.EstId.HasValue && Estilo.Contains(p.EstId.Value)); //Como estilo puede ser nulo, primero verificamos
                //que tenga valor el estid antes de llamar al producto para evitar que haya null
                //algo como: SELECT * FROM Productos WHERE est_id IN (List<int> estilos) AND NOT NULL
            }
            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(p => p.ProGenero.ToLower() == genero.ToLower());
            }
            if (PrecioMin.HasValue)
            {
                query = query.Where(p => p.ProPrecio >= PrecioMin.Value);
            }
            if (PrecioMax.HasValue)
            {
                query = query.Where(p => p.ProPrecio <= PrecioMax.Value);
            }

            //Como talla y color estan dentro de Variantes que es una tabla  hija de Productos se hace más proceso
            if (talla != null && talla.Any()) //si talla no es una lista vacia y tiene algun valor
            {
                                                              //esto es solo mostrar las variantes donde su varTalla esten en la lista
                query = query.Where(p => p.Variantes.Any(v => talla.Select(t => t.ToLower()).Contains(v.VarTalla.ToLower())));
                             //Dame los productos donde almenos una de sus variantes cumpla con la condición, por eso el "Any"
                             //Cuando una de esas variantes cumple con la condicion, mostrará ese producto asi solo una variante cumpla
                             //El select en talla solo es para volverlo a lower
            }
            if (color != null && color.Any())
            { //aplica lo mismo que para talla
                query = query.Where(p => p.Variantes.Any(v => color.Select(c => c.ToLower()).Contains(v.VarColor.ColorNombre.ToLower())));
            }

            if (stock == true)
            {
                query = query.Where(p => p.Variantes.Any(v => v.VarStock > 0));
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
                    Estilo = p.Estilo!= null ? p.Estilo.EstNombre : null, //como estoy buscando por estilo, no será nulo
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
            //a pesar de que pueda no cargar los productos nunca deberia devolver un 404, solo una lista vacia
            return Ok(productos);
        }
    }
}
