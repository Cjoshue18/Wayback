using BackEnd.Data;
using BackEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly FashionShopContext _context;

        public AuthController(FashionShopContext context)
        {
            _context = context;
        }

        [HttpPost("login")] //añade una subruta url/api/login
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if(string.IsNullOrEmpty(loginDto.CliUsername) || string.IsNullOrEmpty(loginDto.CliPasswordHash))
            {
                return BadRequest("Ingrese Usuario y Contraseña.");
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.CliUsername == loginDto.CliUsername);

            if(cliente == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            if(cliente.CliPasswordHash != loginDto.CliPasswordHash)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            return Ok(new
            {
                message = "¡Login exitoso",
                usuario = cliente.CliUsername,
                nombre = cliente.CliName,
                token = "test"
            });
        }

    }
}
