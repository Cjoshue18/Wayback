using BackEnd.Data;
using BackEnd.DTOs.Admin;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/pedidos")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminPedidosController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AdminPedidosController(WaybackContext context)
        {
            _context = context;
        }

        // 1. Obtener todos los pedidos (Resumen)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminPedidoResumenDTO>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .OrderByDescending(p => p.PedFechaCompra)
                .Select(p => new AdminPedidoResumenDTO
                {
                    PedId = p.PedId,
                    PedEstado = p.PedEstado,
                    PedTotal = p.PedTotal,
                    PedFechaCompra = p.PedFechaCompra,
                    PedFechaEntrega = p.PedFechaEntrega,
                    PedMetTipoPago = p.PedMetTipoPago,
                    PedDirCalle = p.PedDirCalle,
                    PedDirDistrito = p.PedDirDistrito,
                    PedDirProvincia = p.PedDirProvincia,
                    PedDirDepartamento = p.PedDirDepartamento,
                    CliNombre = p.Cliente.CliNombre,
                    CliApellido = p.Cliente.CliApellido,
                    UsuEmail = p.Cliente.Usuario.UsuEmail
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        // 2. Obtener el detalle de un pedido específico
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdminPedidoDetalleDTO>> GetPedidoDetalle(int id)
        {
            var pedido = await _context.Pedidos
                .Where(p => p.PedId == id)
                .Select(p => new AdminPedidoDetalleDTO
                {
                    PedId = p.PedId,
                    PedEstado = p.PedEstado,
                    PedTotal = p.PedTotal,
                    PedFechaCompra = p.PedFechaCompra,
                    PedFechaEntrega = p.PedFechaEntrega,
                    PedMetTipoPago = p.PedMetTipoPago,
                    PedMetUltimos4 = p.PedMetUltimos4,
                    PedDirCalle = p.PedDirCalle,
                    PedDirDistrito = p.PedDirDistrito,
                    PedDirProvincia = p.PedDirProvincia,
                    PedDirDepartamento = p.PedDirDepartamento,
                    PedDirReferencia = p.PedDirReferencia,
                    CliNombre = p.Cliente.CliNombre,
                    CliApellido = p.Cliente.CliApellido,
                    UsuEmail = p.Cliente.Usuario.UsuEmail,
                    Detalles = p.Detalles.Select(d => new AdminPedidoDetalleItemDTO
                    {
                        VarId = d.VarId,
                        DetPedCantidad = d.DetPedCantidad,
                        DetPedPrecioUnitario = d.DetPedPrecioUnitario,
                        DetPedSubTotal = d.DetPedSubTotal,
                        ProNombre = d.Variante.Producto.ProNombre,
                        VarTalla = d.Variante.VarTalla,
                        ColorNombre = d.Variante.VarColor.ColorNombre,
                        ImgURL = d.Variante.Imagen != null ? d.Variante.Imagen.ImgURL : null
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (pedido == null)
            {
                return NotFound($"El pedido con id: {id} no existe.");
            }

            return Ok(pedido);
        }

        // 3. Editar la fecha de entrega del pedido
        [HttpPut("{id:int}/fecha-entrega")]
        public async Task<IActionResult> EditarFechaEntregaPedido(int id, [FromBody] AdminUpdateFechaEntregaDTO dto)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.PedId == id);

            if (pedido == null)
            {
                return NotFound($"El pedido con id: {id} no existe.");
            }

            // Solo se puede asignar fecha de entrega si el pedido está aceptado
            if (dto.PedFechaEntrega.HasValue && pedido.PedEstado.ToLower() != "aceptado")
            {
                return Conflict(new { mensaje = $"Solo se puede asignar una fecha de entrega a pedidos en estado 'aceptado'. Estado actual: '{pedido.PedEstado}'." });
            }

            if (dto.PedFechaEntrega.HasValue && dto.PedFechaEntrega.Value < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return BadRequest(new { mensaje = "La fecha de entrega no puede ser una fecha pasada." });
            }

            pedido.PedFechaEntrega = dto.PedFechaEntrega;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                PedId = pedido.PedId,
                PedEstado = pedido.PedEstado,
                PedFechaEntrega = pedido.PedFechaEntrega,
                mensaje = "Fecha de entrega actualizada correctamente."
            });
        }

        // 4. Editar el estado del pedido
        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> EditarEstadoPedido(int id, [FromBody] AdminUpdatePedidoEstadoDTO dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.PedId == id);

            if (pedido == null)
            {
                return NotFound($"El pedido con id: {id} no existe.");
            }

            string estadoAnterior = pedido.PedEstado.ToLower();
            string estadoNuevo = dto.PedEstado.ToLower();

            if (estadoAnterior == estadoNuevo)
            {
                return Ok(new { mensaje = "El pedido ya se encuentra en ese estado." });
            }

            // Un pedido entregado es estado terminal: el producto ya fue despachado físicamente
            if (estadoAnterior == "entregado")
            {
                return Conflict(new { mensaje = "No se puede modificar un pedido que ya fue entregado." });
            }

            // Lista de estados válidos
            var estadosValidos = new[] { "pendiente", "aceptado", "rechazado", "cancelado", "entregado" };
            if (!estadosValidos.Contains(estadoNuevo))
            {
                return BadRequest($"Estado '{dto.PedEstado}' no es válido. Los estados válidos son: {string.Join(", ", estadosValidos)}.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Manejo de Stock según transición de estados
                bool eraInactivo = estadoAnterior == "cancelado" || estadoAnterior == "rechazado";
                bool esInactivo = estadoNuevo == "cancelado" || estadoNuevo == "rechazado";

                if (!eraInactivo && esInactivo)
                {
                    // Si pasa de Activo a Inactivo: Devolver unidades al stock
                    foreach (var item in pedido.Detalles)
                    {
                        var variante = await _context.Variantes
                            .FirstOrDefaultAsync(v => v.VarId == item.VarId);
                        if (variante != null)
                        {
                            variante.VarStock += item.DetPedCantidad;
                        }
                    }
                }
                else if (eraInactivo && !esInactivo)
                {
                    // Si pasa de Inactivo a Activo: Volver a descontar stock y validar que haya suficiente
                    foreach (var item in pedido.Detalles)
                    {
                        var variante = await _context.Variantes
                            .Include(v => v.Producto)
                            .FirstOrDefaultAsync(v => v.VarId == item.VarId);

                        if (variante == null)
                        {
                            return NotFound($"La variante con id {item.VarId} ya no existe.");
                        }

                        if (variante.VarStock < item.DetPedCantidad)
                        {
                            return BadRequest($"No hay suficiente stock para reactivar el pedido. Producto: {variante.Producto.ProNombre} (Talla: {variante.VarTalla}). Stock disponible: {variante.VarStock}.");
                        }

                        variante.VarStock -= item.DetPedCantidad;
                    }
                }

                // Manejo de fecha de entrega
                if (estadoNuevo == "entregado")
                {
                    pedido.PedFechaEntrega = DateOnly.FromDateTime(DateTime.UtcNow);
                }
                else
                {
                    pedido.PedFechaEntrega = null;
                }

                // Actualizar el estado
                pedido.PedEstado = dto.PedEstado;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    PedId = pedido.PedId,
                    PedEstado = pedido.PedEstado,
                    PedFechaEntrega = pedido.PedFechaEntrega,
                    mensaje = "Estado de pedido actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Error al actualizar el estado del pedido.", detalle = ex.Message });
            }
        }

        // 5. Ingresos semanales (últimos 7 días)
        [HttpGet("ingresos-semanales")]
        public async Task<ActionResult<IEnumerable<AdminIngresoDiarioDTO>>> GetIngresosSemanales()
        {
            var fechaInicio = DateTime.UtcNow.Date.AddDays(-6);
            
            var pedidos = await _context.Pedidos
                .Where(p => p.PedEstado.ToLower() == "entregado" && p.PedFechaCompra >= fechaInicio)
                .ToListAsync();

            var ingresos = new List<AdminIngresoDiarioDTO>();
            for (int i = 0; i < 7; i++)
            {
                var fecha = fechaInicio.AddDays(i);
                var total = pedidos.Where(p => p.PedFechaCompra.Date == fecha.Date).Sum(p => p.PedTotal);
                
                // Nombres de dias en español
                var dia = fecha.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
                dia = char.ToUpper(dia[0]) + dia.Substring(1);

                ingresos.Add(new AdminIngresoDiarioDTO
                {
                    Fecha = dia,
                    Total = total
                });
            }

            return Ok(ingresos);
        }
    }
}

