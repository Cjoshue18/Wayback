using BackEnd.Data;
using BackEnd.DTOs.Auth;
using BackEnd.Models;
using BCrypt.Net;
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

        [HttpPost("register-cliente")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO nuevoCliente)
        {
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuEmail == nuevoCliente.Email); //ver si el email ya existe en
            //la base de datos
            if (emailExiste)
            {
                return Conflict("Este email ya está registrado."); //Error 409
            }

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuUsername == nuevoCliente.NombreUsuario);
            if (usuarioExiste)
            {
                return Conflict("Este nombre de usuario ya está registrado.");
            }
            var documentoExiste = await _context.Clientes
                .AnyAsync(c => c.CliDocumento == nuevoCliente.Documento);
            if (documentoExiste)
            {
                return Conflict("Este documento de identidad ya está registrado");
            }
            //inicio transaccion:
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //registrar usuario:
                var usuario = new Usuarios
                {
                    UsuEmail = nuevoCliente.Email,
                    UsuUsername = nuevoCliente.NombreUsuario,
                    UsuContrasenaHash = BCrypt.Net.BCrypt.HashPassword(nuevoCliente.Contrasena, workFactor: 11),
                    UsuRol = "cliente"
                };
                _context.Add(usuario);
                await _context.SaveChangesAsync(); //una vez guardado, ya tendremos un id de usuario
                //registrar cliente:
                var cliente = new Clientes
                {
                    UsuId = usuario.UsuId, //fk
                    CliNombre = nuevoCliente.Nombres,
                    CliApellido = nuevoCliente.Apellidos,
                    CliTipoDocumento = nuevoCliente.TipoDocumento,
                    CliDocumento = nuevoCliente.Documento
                };
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                //finaliza la transaccion
                return Ok("Usuario Registrado Correctamente");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); //si ocurre un error no se aplica
                //ningun cambio y se regresa al estado previo.
                return StatusCode(500, new { message = "Error al registrar cliente."});
            }
        }
        
        [HttpPost("login")] //añade una subruta url/api/login
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(c => c.UsuUsername == loginDto.UsuUsernameOrEmail
                || c.UsuEmail == loginDto.UsuUsernameOrEmail);

            if(usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }
            var valido = BCrypt.Net.BCrypt.Verify(loginDto.UsuContrasena, usuario.UsuContrasenaHash); 
            if(!valido)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            return Ok(new
            {
                message = "¡Login exitoso",
                usuario = usuario.UsuUsername,
                token = "test",
                Rol = usuario.UsuRol
            });
        }
    }
}
