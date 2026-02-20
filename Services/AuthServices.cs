using BCrypt.Net;
using InventarioProyecto.Data;
using InventarioProyecto.Models;
using InventarioProyecto.Repositories;
using Microsoft.EntityFrameworkCore;

public class AuthService(AppDbContext context) : IAuthService
{
    public async Task<string> Registrar(Usuario user, string password)
    {
        // 1. Verificar si el email existe
        if (await context.Usuarios.AnyAsync(u => u.Email == user.Email))
            throw new Exception("El usuario ya existe");

        // 2. Encriptar contraseña
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        context.Usuarios.Add(user);
        await context.SaveChangesAsync();
        return "Registro exitoso";
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        
        // 3. Verificar usuario y comparar Hash
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Credenciales incorrectas");

        // Aquí luego generaremos un JWT. Por ahora, devolvemos un mensaje.
        return "Login exitoso";
    }

    public async Task<bool> SolicitarRecuperacion(string email)
    {
        var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;
        
        await context.SaveChangesAsync();
        // Aquí enviarías un email con el token...
        return true;
    }
}