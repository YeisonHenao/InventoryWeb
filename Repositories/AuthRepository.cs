using InventarioProyecto.Data;
using InventarioProyecto.Models;
using Microsoft.EntityFrameworkCore;
using InventarioProyecto.Utils;
using Microsoft.Extensions.Configuration;

namespace InventarioProyecto.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext context;
    private readonly Jwt jwt;
    private readonly Bcrypt bcrypt;

    public AuthRepository(AppDbContext context, IConfiguration config)
    {
        this.context = context;
        this.jwt = new Jwt(config);
        this.bcrypt = new Bcrypt();
    }

    // TODO: Pendiente crear esta validación del usuario
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
                throw new AuthException(ErrorTypesAuth.EmailAlreadyRegistered, "El correo electrónico ya está registrado.");
            }

            var hashPassword = Bcrypt.HashPassword(entity.Password);
            var user = new Usuario
            {
                Nombre = entity.Nombre,
                PasswordHash = hashPassword,
                Email = entity.Email,
                Estado = 1,
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = null
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

    public async Task<UsuarioResponse> Login(string Email, String Password)
    {
        try
        {
            // Buscar si existe el usuario
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email);
            // Si no existe, lanzar una excepción
            if (usuario == null)
            {
                throw new AuthException(ErrorTypesAuth.UserNotFound, "Usuario no encontrado.");
            }
            else
            {
                // Si la contraseña no es válida, lanzar una excepción
                if (Bcrypt.VerifyPassword(Password, usuario.PasswordHash) == false)
                {
                    throw new AuthException(ErrorTypesAuth.InvalidCredentials, "Credenciales inválidas.");
                }
                else
                {
                    var actualizado = await ActualizarAccesoUsuario(usuario);
                    if (!actualizado)
                    {
                        throw new AuthException(ErrorTypesAuth.UserAccessUpdateFailed, "Error al actualizar el acceso del usuario.");
                    }

                    string token = jwt.GenerarToken(usuario);
                    if (string.IsNullOrEmpty(token))
                    {
                        throw new AuthException(ErrorTypesAuth.TokenGenerationFailed, "Error al generar el token de autenticación.");
                    }

                    UsuarioResponse response = new UsuarioResponse
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        Email = usuario.Email,
                        PasswordHash = usuario.PasswordHash,
                        Estado = usuario.Estado,
                        FechaCreacion = usuario.FechaCreacion,
                        UltimoAcceso = usuario.UltimoAcceso,
                        Token = token
                    };

                    return response;

                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    // TODO: Pendiente crear esta petición
    public async Task<String> SolicitarRecuperacion(string email)
    {
        try
        {
            var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new AuthException(ErrorTypesAuth.EmailNotRegistered, "El correo electrónico no está registrado.");
            }

            return "Correo de recuperación enviado";
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<bool> ActualizarAccesoUsuario(Usuario user)
    {
        try
        {
            user.UltimoAcceso = DateTime.UtcNow;
            context.Usuarios.Update(user);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en ActualizarAccesoUsuario: {ex.Message}");
            return false;
        }
    }
}