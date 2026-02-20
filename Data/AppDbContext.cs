using Microsoft.EntityFrameworkCore;
using InventarioProyecto.Models;

namespace InventarioProyecto.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; } // La nueva tabla

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            // Email único para evitar duplicados en el registro
            entity.HasIndex(u => u.Email).IsUnique();
            
            // Configurar que el Id sea generado automáticamente si es un Guid nuevo
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
        });

        // 2. Configuración de Producto (opcional, para mantener orden)
        modelBuilder.Entity<Producto>().HasKey(p => p.Id);
    }
}