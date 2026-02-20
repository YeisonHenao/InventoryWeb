using System.ComponentModel.DataAnnotations;

namespace InventarioProyecto.Models;

public class Producto
{
    [Key] // Esto le dice a SQLite que es la Clave Primaria y Autoincremental
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Sku { get; set; } = string.Empty; // Código único de inventario

    public decimal Precio { get; set; }

    public int StockActual { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}