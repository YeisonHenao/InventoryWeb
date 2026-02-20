using InventarioProyecto.Models;

namespace InventarioProyecto.Services;

public interface IProductoService 
{
    Task<IEnumerable<Producto>> ObtenerTodo();
}