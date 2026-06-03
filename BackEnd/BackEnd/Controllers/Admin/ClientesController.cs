using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly FashionShopContext _context;
        
        public ClientesController(FashionShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Usuario) //Incluye la información del usuario relacionado con cada cliente
                .ToListAsync(); //obtiene la lista de clientes de la base de datos
            return Ok(clientes); //Lista de todos los objetos Clientes
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetClienteByID(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario) //Incluye la información del usuario relacionado con cada cliente
                .FirstOrDefaultAsync(c => c.CliId == id); //busca y lo asigna a la variable
            if (cliente == null)
            {
                return NotFound(); //si no lo encuentra devuelve no encontrado
            }
            return Ok(cliente); //Si lo encuentra devuelve el Cliente
        }

        [HttpPost]
        public async Task<ActionResult<Clientes>> AddCliente([FromBody] Clientes nuevoCliente)
        {
            if(nuevoCliente == null || nuevoCliente.Usuario == null)
            {
                return BadRequest();
            }
            _context.Clientes.Add(nuevoCliente); //añado el nuevo cliente al contexto (tabla)
            await _context.SaveChangesAsync(); //guardo los cambios
            return CreatedAtAction(nameof(GetClienteByID), new {id = nuevoCliente.CliId }, nuevoCliente ); 
            //muestro lo que acabo de agregar
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Clientes upCliente)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.CliId == id); //encontrar por Id
            if(cliente == null)
            {
                return NotFound();
            }
            cliente.CliDocument = upCliente.CliDocument;
            cliente.CliDocumentType = upCliente.CliDocumentType;
            cliente.CliName = upCliente.CliName;
            cliente.CliLastName = upCliente.CliLastName;
            cliente.CliPhone = upCliente.CliPhone;
            cliente.CliPasarelaId = upCliente.CliPasarelaId;
            
            //Si tambien buscamos cambiar sus datos en la tabla Usuarios:
            if(cliente.Usuario != null && upCliente.Usuario != null)
            {
                cliente.Usuario.UsuEmail = upCliente.Usuario.UsuEmail;
                cliente.Usuario.UsuUsername = upCliente.Usuario.UsuUsername;
            }
            await _context.SaveChangesAsync(); //guardar los cambios
            return NoContent(); //no retorna contenido
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
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