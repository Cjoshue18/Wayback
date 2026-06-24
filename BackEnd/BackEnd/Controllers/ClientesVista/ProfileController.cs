using BackEnd.Data;
using BackEnd.DTOs.ClientesVista;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;

namespace BackEnd.Controllers.ClientesVista
{
    [Route("api/[controller]/mi-perfil")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly WaybackContext _context;

        public ProfileController(WaybackContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<ProfileDTO>> GetPerfil()
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            //User es una propiedad de los Claim, representa el usuario verificado con JWT que que vino en el Bearer
            //FindFirst encontrara el tipode claim identificador que asignamos a usuID y recogerá su VALUE, usamos ! para que no bote
            //posible nullreference porque SABEMOS que no dara null.

            var cliente = await _context.Clientes
                .Where(c => c.UsuId == usuId)
                .Select(c => new ProfileDTO
                {
                    CliNombre = c.CliNombre,
                    CliApellido = c.CliApellido,
                    CliTelefono = c.CliTelefono,
                    UsuUsername = c.Usuario.UsuUsername,
                    UsuEmail = c.Usuario.UsuEmail
                })
                .FirstOrDefaultAsync(); //solo recuperamos 1 cliente pero igual por buena practica usamos esto
                //para poder luego ver si es nulo o no, la base de datos de por si ya garantiza unicidad.

            if(cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }
        [HttpPut]
        public async Task<IActionResult> ActualizarDatos([FromBody] EditarDatosClientesDTO upCliente)
        {
            var usuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuId == usuId); //encontrar por Id de usuario
            if (cliente == null)
            {
                return NotFound();
            }

            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuEmail == upCliente.UsuEmail && u.UsuId != cliente.UsuId);
            if (emailExiste)
            {
                return Conflict("Este email ya está registrado.");
            }

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuUsername == upCliente.UsuUsername && u.UsuId != cliente.UsuId);
            if (usuarioExiste)
            {
                return Conflict("Este nombre de usuario ya está registrado.");
            }
            //se añade && UsuId para que no compare consigo mismo por si no se envia nada en el formulario y no salte la condicion


            cliente.CliNombre = upCliente.CliNombre;
            cliente.CliApellido = upCliente.CliApellido;
            cliente.CliTelefono = upCliente.CliTelefono;
            cliente.Usuario.UsuUsername = upCliente.UsuUsername;
            cliente.Usuario.UsuEmail = upCliente.UsuEmail;
            await _context.SaveChangesAsync(); //guardar los cambios
            return NoContent(); //no retorna contenido
        }
    }
}
