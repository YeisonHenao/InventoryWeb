using InventarioProyecto.Data;
using InventarioProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioProyecto.Repositories;

public class ProductoRepository(AppDbContext context) : IProductoRepository
{
    public async Task<IEnumerable<Producto>> GetAllAsync() 
        => await context.Productos.ToListAsync();

    public async Task AddAsync(Producto producto)
    {
        await context.Productos.AddAsync(producto);
        await context.SaveChangesAsync();
    }
}