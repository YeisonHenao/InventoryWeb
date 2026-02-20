using Microsoft.EntityFrameworkCore;
using InventarioProyecto.Models;

namespace InventarioProyecto.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Esta es la representación de tu tabla "Productos" en C#
    public DbSet<Producto> Productos { get; set; }
}