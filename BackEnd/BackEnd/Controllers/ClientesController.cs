using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        [HttpGet]
        public ActionResult GetClientes()
        {
            return Ok(new { message = "Lista" });
        }
    }
}