using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
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
            return Ok(await _context.Clientes.ToListAsync()); //Lista de todos los objetos Clientes
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetClienteByID(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id); //busca y lo asigna a la variable
            if (cliente == null)
            {
                return NotFound(); //si no lo encuentra devuelve no encontrado
            }
            return Ok(cliente); //Si lo encuentra devuelve el Cliente
        }

        [HttpPost]
        public async Task<ActionResult<Clientes>> AddCliente([FromBody] Clientes nuevoCliente)
        {
            if(nuevoCliente == null)
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
            var cliente = await _context.Clientes.FindAsync(id); //encontrar por Id
            if(cliente == null)
            {
                return NotFound();
            }
            cliente.CliDocument = upCliente.CliDocument;
            cliente.CliDocumentType = upCliente.CliDocumentType;
            cliente.CliName = upCliente.CliName;
            cliente.CliLastName = upCliente.CliLastName;
            cliente.CliEmail = upCliente.CliEmail;
            cliente.CliPhone = upCliente.CliPhone;
            cliente.CliUsername = upCliente.CliUsername;
            cliente.CliPasswordHash = upCliente.CliPasswordHash;
            cliente.CliStripeId = upCliente.CliStripeId;
            cliente.CliRegisterDate = upCliente.CliRegisterDate;
            await _context.SaveChangesAsync(); //guardar los cambios
            return NoContent(); //no retorna contenido
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if(cliente == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}