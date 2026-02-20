using InventarioProyecto.Models;

namespace InventarioProyecto.Repositories;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task AddAsync(Producto producto);
}