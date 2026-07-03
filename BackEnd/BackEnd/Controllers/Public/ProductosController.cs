using BackEnd.Data;
using BackEnd.DTOs;
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
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo != null ? p.Estilo.EstNombre : null,
                    Variantes = p.Variantes.Select(v => new VariantesDetalleDTO
                    {
                        VarId = v.VarId,
                        ColorNombre = v.VarColor.ColorNombre,
                        ColorHex = v.VarColor.ColorHex,
                        VarTalla = v.VarTalla,
                        VarStock = v.VarStock,
                        VarImgUrl = v.Imagen != null ? v.Imagen.ImgURL : null
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (dto == null) return NotFound();

            return Ok(dto);
        }


        [HttpGet]
        public async Task<ActionResult<ListaPaginada<ProductoTarjetaDTO>>> GetProductoFilter(
            [FromQuery] int pagina = 1, [FromQuery] int registrosPorPagina = 10,
            [FromQuery] List<int>? Categoria = null,
            [FromQuery] List<int>? Estilo = null, [FromQuery] string? genero = null, [FromQuery] decimal? PrecioMin = null, [FromQuery] decimal? PrecioMax = null,
            [FromQuery] List<int>? color = null, [FromQuery] List<string>? talla = null, [FromQuery] bool? stock = null) 
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
            {
                query = query.Where(p => p.Variantes.Any(v => color.Contains(v.VarColor.ColorId)));
            }

            if (stock == true)
            {
                query = query.Where(p => p.Variantes.Any(v => v.VarStock > 0));
            }

            var totalRegistros = await query.CountAsync();

            var productos = await query
                .OrderBy(p => p.ProId)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(p => new ProductoTarjetaDTO
                {
                    ProId = p.ProId,
                    ProNombre = p.ProNombre,
                    ProPrecio = p.ProPrecio,
                    ProDescuento = p.ProDescuento,
                    ProDescuentoInicio = p.ProDescuentoInicio,
                    ProDescuentoFin = p.ProDescuentoFin,
                    Categoria = p.Categoria.CatNombre,
                    Estilo = p.Estilo!= null ? p.Estilo.EstNombre : null, //como estoy buscando por estilo, no será nulo
                    Colores = p.Variantes
                        .Where(v => v.VarStock > 0)
                        .GroupBy(v => v.VarColor.ColorHex) //Agrupamos porque variantes (S, M, L, etc.) se refieren a un color
                        .Select(g => new ColorTarjetaDTO //como se agrupo con Group By ahora tiene propiedad .key que es la columna que se uso para agrupar, y todas las propiedades de la clase ColorTarjetaDTO
                        { 
                            ColorHex = g.Key, //es el colorhex, columna con la cual se agrupo
                            ImgUrl = g.Where(v => v.Imagen != null) //Nos aseguramos de que nos de una imagen no nula
                                        .Select(v => v.Imagen.ImgURL).FirstOrDefault() //como son varias variantes en el grupo, solo queremos el primer color
                        }) //como es primero o por defecto, te da una imagen y si no el defecto de un string es un null
                        .Take(5)
                        .ToList()

                }).ToListAsync();

            var result = new ListaPaginada<ProductoTarjetaDTO>
            {
                TotalRegistros = totalRegistros,
                PaginaActual = pagina,
                RegistrosPorPagina = registrosPorPagina,
                Elementos = productos
            };

            //a pesar de que pueda no cargar los productos nunca deberia devolver un 404, solo una lista vacia
            return Ok(result);
        }
    }
}
