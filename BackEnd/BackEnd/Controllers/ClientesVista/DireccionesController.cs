using BackEnd.Data;
using BackEnd.DTOs.ClientesVista;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackEnd.Controllers.ClientesVista
{
    [Route("api/profile/[controller]")]
    [ApiController]
    [Authorize]
    public class DireccionesController : ControllerBase
    {
        private readonly WaybackContext _context;

        public DireccionesController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DireccionesDTO>>> GetDirecciones()
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var direcciones = await _context.Clientes
                .Where(c => c.UsuId == usuId)
                .SelectMany(d => d.Direcciones) //Para evitar un JSON con Lista de listas [0: [dircalle, dirdistrito, etc.]]
                                                //Asi solo llega [dircalle, dirdistrito, etc]
                .Select(d => new DireccionesDTO //siempre va a devolver varios clientes asi quede 1 y hara lista de listas, asi que selectMany es necesario
                {
                    DirCalle = d.DirCalle,
                    DirDistrito = d.DirDistrito,
                    DirProvincia = d.DirProvincia,
                    DirDepartamento = d.DirDepartamento,
                    DirReferencia = d.DirReferencia,
                    DirPreferido = d.DirPreferido
                })
                .ToListAsync();
            return Ok(direcciones); //siempre devolver asi no haya direcciones para no botar error 404
        }
        [HttpPost]
        public async Task<IActionResult> CrearDireccion([FromBody] UpsertDireccionesDTO dto)
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuId == usuId);

            if (cliente == null)
            {
                return NotFound();
            }

            if (dto.DirPreferido) //si la nueva direccion es favorita
            {
                var OtrasDirecciones = await _context.Direcciones
                    .Where(d => d.CliId == cliente.CliId) //todas las demas direcciones del cliente
                    .ToListAsync();

                foreach (var item in OtrasDirecciones)
                {
                    item.DirPreferido = false;
                }
            } //Todas las demas direcciones se volveran No favoritas

            var nuevaDireccion = new Direcciones
            {
                CliId = cliente.CliId,
                DirCalle = dto.DirCalle,
                DirDistrito = dto.DirDistrito,
                DirProvincia = dto.DirProvincia,
                DirDepartamento = dto.DirDepartamento,
                DirReferencia = dto.DirReferencia,
                DirPreferido = dto.DirPreferido
            };

            _context.Direcciones.Add(nuevaDireccion);
            await _context.SaveChangesAsync();

            return Ok(new { dirId = nuevaDireccion.DirId }); //devuelve el nuevo id
        }


        [HttpPut("{dirId:int}")]
        public async Task<IActionResult> EditarDireccion(int dirId, [FromBody] UpsertDireccionesDTO dto)
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuId == usuId);

            if (cliente == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.DirId == dirId && d.CliId == cliente.CliId);
            //Encuentra la direccion seleccionada Y si le pertenece a ese cliente o no

            if (direccion == null) return NotFound(); //si no existe la direccion o no es de ese cliente.

            //Si la direccion editada ahora se vuelve la nueva favorita(solo se puede tener una favorita)
            //Y antes no era la favorita
            if (dto.DirPreferido && !direccion.DirPreferido)
            {
                var OtrasDirecciones = await _context.Direcciones
                    .Where(d => d.CliId == cliente.CliId && d.DirId != dirId) //todas las demas direcciones del cliente
                    .ToListAsync();

                foreach (var item in OtrasDirecciones)
                {
                    item.DirPreferido = false;
                }
            } //Todas las demas direcciones se volveran No favoritas
            direccion.DirCalle = dto.DirCalle;
            direccion.DirDistrito = dto.DirDistrito;
            direccion.DirProvincia = dto.DirProvincia;
            direccion.DirDepartamento = dto.DirDepartamento;
            direccion.DirReferencia = dto.DirReferencia;
            direccion.DirPreferido = dto.DirPreferido;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{dirId:int}")]
        public async Task<IActionResult> EliminarDireccion(int dirId)
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuId == usuId);

            if (cliente == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.DirId == dirId && d.CliId == cliente.CliId);
            if (direccion == null) return NotFound();

            _context.Direcciones.Remove(direccion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
