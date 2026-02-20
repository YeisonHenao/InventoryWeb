using Microsoft.AspNetCore.Mvc;
using InventarioProyecto.Services;

namespace InventarioProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController(IProductoService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await service.ObtenerTodo());
    }
}