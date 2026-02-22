using InventarioProyecto.Data;
using InventarioProyecto.Models;
using Microsoft.EntityFrameworkCore;
using InventarioProyecto.Utils;

namespace InventarioProyecto.Repositories;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    public async Task<Usuario> AuthenticateAsync(string username, string password)
    {
        try
        {
            var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en AuthenticateAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<Usuario> Register(UserDto entity)
    {
        try
        {
            var existe = await context.Usuarios.AnyAsync(u => u.Email == entity.Email);
            if (existe)
            {
                throw new Exception("El correo electrónico ya está registrado.");
            }

            var hashPassword = Bcrypt.HashPassword(entity.Password);
            var user = new Usuario
            {
                Nombre = entity.Nombre,
                PasswordHash = hashPassword,
                Email = entity.Email
            };

            await context.Usuarios.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Usuario> Login(string Nombre, String Password)
    {
        try
        {
            var hashPassword = Bcrypt.HashPassword(Password);
            var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == Nombre && u.PasswordHash == hashPassword);
            if (user != null)
            {
                var sesionActualizada = await ActualizarAccesoUsuario(user);
                if (!sesionActualizada)
                {
                    Console.WriteLine("Error al actualizar la sesión del usuario.");
                }
                return user;
            }
            else
            {
                throw new Exception("Credenciales inválidas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en Login: {ex.Message}");
            return null;
        }
    }

    public async Task<String> SolicitarRecuperacion(string email)
    {
        try
        {
            var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return "Usuario no encontrado";

            // Aquí podrías generar un token de recuperación y enviarlo por correo
            return "Correo de recuperación enviado";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en SolicitarRecuperacion: {ex.Message}");
            return "Error al solicitar recuperación";
        }
    }

    private async Task<bool> ActualizarAccesoUsuario(Usuario user)
    {
        var actualizado = new Usuario
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Estado = user.Estado,
            FechaCreacion = user.FechaCreacion,
            UltimoAcceso = DateTime.UtcNow,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
            SecurityStamp = user.SecurityStamp
        };

        context.Usuarios.Update(actualizado);
        var result = await context.SaveChangesAsync();
        return result > 0;
    }
}