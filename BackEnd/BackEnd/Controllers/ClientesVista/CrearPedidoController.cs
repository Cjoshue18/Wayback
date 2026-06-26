using BackEnd.Data;
using BackEnd.DTOs.ClientesVista;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackEnd.Controllers.ClientesVista
{
    [Route("api/mis-pedidos")]
    [ApiController]
    [Authorize]
    public class CrearPedidoController : ControllerBase
    {
        private readonly WaybackContext _context;

        public CrearPedidoController(WaybackContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> GenerarOrden([FromBody] CrearPedidoYapeDTO dto)
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuId == usuId);

            if (cliente == null) return NotFound();

            var direccion = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.DirId == dto.DirId && d.CliId == cliente.CliId);

            if (direccion == null) return BadRequest("Direccion no válida.");

            //creamos variables para almacenar los datos de los items del carrito
            decimal total = 0;
            var detallesValidados = new List<(Variantes variante, int cantidad, decimal precioU)>();


            foreach(var item in dto.Items)
            {
                var variante = await _context.Variantes
                    .Include(v => v.Producto) //Para obtener los valores del producto padre
                    .FirstOrDefaultAsync(v => v.VarId == item.VarId);

                if (variante == null) return NotFound($"La variante {item.VarId} no existe.");

                if (variante.VarStock < item.Cantidad || variante.VarStock <= 0)
                {
                    return BadRequest($"La cantidad pedida del producto {variante.Producto.ProNombre} excede el stock disponible.");
                }

                var precioU = variante.Producto.ProPrecio;
                total += precioU * item.Cantidad;

                //agrego los detalles despues de validar a la lista, usando tuplas:
                detallesValidados.Add((variante, item.Cantidad, precioU));
                    
            }
            //comenzamos una transaccion
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pedido = new Pedidos
                {
                    CliId = cliente.CliId,
                    MetId = null,
                    DirId = dto.DirId,
                    PedEstado = "pendiente",
                    PedTotal = total,
                    PedPasarelaCargoId = null,
                    PedFechaCompra = DateTime.UtcNow,
                    PedFechaEntrega = null,
                    //snapshot de la direccion
                    PedDirCalle = direccion.DirCalle,
                    PedDirDistrito = direccion.DirDistrito,
                    PedDirProvincia = direccion.DirProvincia,
                    PedDirDepartamento = direccion.DirDepartamento,
                    PedDirReferencia = direccion.DirReferencia,
                    PedMetTipoPago = "Yape",
                    PedMetUltimos4 = null,
                    Detalles = detallesValidados.Select(d => new PedidoDetalles
                    {
                        VarId = d.variante.VarId,
                        DetPedPrecioUnitario = d.precioU,
                        DetPedCantidad = d.cantidad,
                        DetPedSubTotal = d.precioU * d.cantidad
                    }).ToList()
                };

                _context.Pedidos.Add(pedido);

                //descontar stock, usando tuplas
                foreach (var (variante, cantidad, subtotal) in detallesValidados)
                {
                    variante.VarStock -= cantidad; //como guarde la clase Variante en cada elemento, puedo acceder a su referencia
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new //Un ok para confirmar que el pedido se realizo con exito
                {
                    PedId = pedido.PedId,
                    PedTotal = pedido.PedTotal,
                    PedEstado = pedido.PedEstado,
                    PedMetTipoPago = "Yape",
                    Mensaje = "¡Pedido confirmado!"
                });
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new {mensaje = "Error al registrar pedido.", detalle = ex.Message});
            }
        }

    }
}
