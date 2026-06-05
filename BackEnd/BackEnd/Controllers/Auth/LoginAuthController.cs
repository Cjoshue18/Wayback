using BackEnd.Data;
using BackEnd.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WaybackContext _context;

        public AuthController(WaybackContext context)
        {
            _context = context;
        }
        /*
        [HttpPost("login")] //añade una subruta url/api/login
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if(string.IsNullOrEmpty(loginDto.UsuUsername) || string.IsNullOrEmpty(loginDto.UsuPasswordHash))
            {
                return BadRequest("Ingrese Usuario y Contraseña.");
            }

            var cliente = await _context.Usuarios
                .Include(u => u.Cliente) // Incluye la entidad relacionada Cliente
                .FirstOrDefaultAsync(c => c.UsuUsername == loginDto.UsuUsername);

            if(cliente == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            if(cliente.UsuPasswordHash != loginDto.UsuPasswordHash)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            return Ok(new
            {
                message = "¡Login exitoso",
                usuario = cliente.UsuUsername,
                nombre = cliente.Cliente!.CliName,
                token = "test"
            });
        }
        */
    }
}
