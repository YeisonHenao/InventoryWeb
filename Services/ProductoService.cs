using InventarioProyecto.Models;
using InventarioProyecto.Repositories;

namespace InventarioProyecto.Services;

public class ProductoService(IProductoRepository repository) : IProductoService
{
    public async Task<IEnumerable<Producto>> ObtenerTodo() 
        => await repository.GetAllAsync();
}