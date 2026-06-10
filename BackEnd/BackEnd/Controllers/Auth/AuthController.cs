using BackEnd.Data;
using BackEnd.DTOs.Auth;
using BackEnd.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WaybackContext _context;
        private readonly IConfiguration _configuration;
        //es IConfiguration porque es la interfaz que nos permite acceder a la configuración de la aplicación
        //para acceder a la configuración del JWT, como el secret key, issuer, audience y lo demas

        public AuthController(WaybackContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //dentro del constructor se usa la configuración para poder acceder a los valores del JWT desde appsettings.json
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
                return StatusCode(500, new { message = "Error al registrar cliente." });
            }
        }

        [HttpPost("login")] //añade una subruta url/api/login
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(c => c.UsuUsername == loginDto.UsuUsernameOrEmail
                || c.UsuEmail == loginDto.UsuUsernameOrEmail);

            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }
            var valido = BCrypt.Net.BCrypt.Verify(loginDto.UsuContrasena, usuario.UsuContrasenaHash);
            if (!valido)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = GenerarToken(usuario);

            return Ok(new
            {
                tokenJWT = token,
                message = "¡Login exitoso",
                usuario = usuario.UsuUsername,
                Rol = usuario.UsuRol
            });
        }

        private string GenerarToken(Usuarios usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            //se crea una clave simétrica a partir del secret key definido en la configuración, que se usará para firmar el token
            var tokenDescriptor = new SecurityTokenDescriptor //usando descriptor como estandar
            {
                Subject = new ClaimsIdentity( //claims son la información que queremos incluir en el token
                [
                    new Claim("id", usuario.UsuId.ToString()), //id de usuario como claim como string
                    new Claim("username", usuario.UsuUsername),
                    new Claim("email", usuario.UsuEmail),
                    new Claim(ClaimTypes.Role, usuario.UsuRol) //rol importante para las autorizaciones
                    //Se usa el ClaimType Role para poder usarlo en authorize
                ]),
                Expires = DateTime.UtcNow.AddDays(7), //el token expira en 7 dias
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                //se especifica que el token se firmará usando la clave simétrica y el algoritmo HMAC SHA256
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor); //el handler crea el token usando el descriptor
            return tokenHandler.WriteToken(token); //el handler lo convierte en un string y lo retorna
        }
    }
}
