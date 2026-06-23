using BackEnd.Data;
using BackEnd.DTOs.ClientesVista;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.ClientesVista
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly WaybackContext _context;

        public ProfileController(WaybackContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("mi-perfil")]
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
    }
}
