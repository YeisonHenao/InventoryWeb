using System.ComponentModel.DataAnnotations;

namespace InventarioProyecto.Models;

public class Usuario
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); // Genera el UUID automáticamente

    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // Estado: 0 = Inactivo, 1 = Activo, 2 = Bloqueado
    public int Estado { get; set; } = 1; 

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? UltimoAcceso { get; set; }

    /* --- CAMPOS PARA JWT MODERNO --- */

    // Se usa para refrescar el Access Token sin pedir credenciales
    public string? RefreshToken { get; set; } 

    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Útil para invalidar todas las sesiones si el usuario cambia la clave
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
}