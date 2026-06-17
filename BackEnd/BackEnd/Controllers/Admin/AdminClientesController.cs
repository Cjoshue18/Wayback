using BackEnd.Data;
using BackEnd.DTOs.Admin;
using BackEnd.DTOs.Cliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/reportes/clientes")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminClientesController : ControllerBase
    {
        private readonly WaybackContext _context;
        
        public AdminClientesController(WaybackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminGetClientesDTO>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Usuario)
                .ToListAsync(); //obtengo los datos en memoria de la base de datos

            if(!clientes.Any())
            {
                return Ok(new List<AdminGetClientesDTO>());
            }

            //Lo convierto en JSON usando usando las DTO para evitar referencia circular
            var dto = clientes.Select(c => new AdminGetClientesDTO
            {
                CliId = c.CliId,
                CliDocumento = c.CliDocumento,
                CliTipoDocumento = c.CliTipoDocumento,
                CliNombre = c.CliNombre,
                CliApellido = c.CliApellido,
                CliTelefono = c.CliTelefono,
                Usuario = new AdminUsuariosDTO
                {
                    UsuId = c.Usuario.UsuId,
                    UsuUsername = c.Usuario.UsuUsername,
                    UsuEmail = c.Usuario.UsuEmail,
                    UsuFechaRegistro = c.Usuario.UsuFechaRegistro
                }
            }).ToList(); //lo convierto en lista al ser muchos registros

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminGetClientesDTO>> GetClienteByID(int id)
        {
            var dto = await _context.Clientes
                .Where(c => c.CliId == id)
                .Select(cliente => new AdminGetClientesDTO //si lo encuentro, asigna los datos en el dto
                {
                    CliId = cliente.CliId,
                    CliDocumento = cliente.CliDocumento,
                    CliTipoDocumento = cliente.CliTipoDocumento,
                    CliNombre = cliente.CliNombre,
                    CliApellido = cliente.CliApellido,
                    CliTelefono = cliente.CliTelefono,
                    Usuario = new AdminUsuariosDTO
                    {
                        UsuId = cliente.Usuario.UsuId,
                        UsuUsername = cliente.Usuario.UsuUsername,
                        UsuEmail = cliente.Usuario.UsuEmail,
                        UsuFechaRegistro = cliente.Usuario.UsuFechaRegistro
                    }
                }).FirstOrDefaultAsync(); //Se ejecuta como una sola línea de SQL
            if (dto == null) return NotFound();
            
            return Ok(dto); //Si lo encuentra devuelve el Cliente
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarCliente(int id, [FromBody] EditarDatosClientesDTO upCliente)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.CliId == id); //encontrar por Id
            if(cliente == null)
            {
                return NotFound();
            }

            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuEmail == upCliente.Email && u.UsuId != cliente.UsuId); 
            if (emailExiste)
            {
                return Conflict("Este email ya está registrado.");
            }

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuUsername == upCliente.NombreUsuario && u.UsuId != cliente.UsuId);
            if (usuarioExiste)
            {
                return Conflict("Este nombre de usuario ya está registrado.");
            }
            //se añade && UsuId para que no compare consigo mismo por si no se envia nada en el formulario y no salte la condicion


            cliente.CliNombre = upCliente.Nombres;
            cliente.CliApellido = upCliente.Apellidos;
            cliente.CliTelefono = upCliente.Telefono;
            cliente.Usuario.UsuUsername = upCliente.NombreUsuario;
            cliente.Usuario.UsuEmail = upCliente.Email;
            await _context.SaveChangesAsync(); //guardar los cambios
            return NoContent(); //no retorna contenido
        }

        [HttpDelete("{id}")] //Hard DELETE, no usar si no es necesario, solo para pruebas
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.CliId == id);
            if(cliente == null)
            {
                return NotFound();
            }
            //Si eliminamos el cliente se elimina su usuario:
            if (cliente.Usuario != null)
            {
                _context.Usuarios.Remove(cliente.Usuario);
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}