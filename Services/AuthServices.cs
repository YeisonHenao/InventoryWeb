using InventarioProyecto.Repositories;
using InventarioProyecto.Models;

namespace InventarioProyecto.Services;

public class AuthService(IAuthRepository repository) : IAuthService
{
    public async Task<Response> Login(string email, string password)
    {
        try
        {
            var user = await repository.Login(email, password);
            if(user == null)
            {
                throw new Exception("Credenciales inválidas.");
            }
            else
            {
                return new Response { status = 1, Message = "Login exitoso", Data = user };
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error durante el login: " + ex.Message);
        }
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema. Verifica si el correo electrónico ya está registrado y, si no, crea un nuevo usuario con la contraseña hasheada. Devuelve el usuario registrado o lanza una excepción si ocurre algún error durante el proceso.
    /// </summary>
    /// <param name="user">El DTO de usuario que contiene los datos del nuevo usuario a registrar.</param>
    /// <returns>El usuario registrado si el proceso es exitoso.</returns>
    /// <exception cref="Exception">Lanzada si ocurre un error durante el registro del usuario.</exception>
    public async Task<Response> Registrar(UserDto user)
    {
        try
        {
            var result = await repository.Register(user);
            if (result == null)
            {
                throw new Exception("No se pudo registrar al usurio");
            }
            else
            {
                return new Response { status = 1, Message = "Usuario registrado exitosamente", Data = result };
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error durante el registro: " + ex.Message);
        }
    }

    public Task<string> SolicitarRecuperacion(string email)
    {
        try
        {
            var result = repository.SolicitarRecuperacion(email);
            return Task.FromResult("Correo");
        }
        catch (Exception ex)
        {
            throw new Exception("Error durante la solicitud de recuperación: " + ex.Message);
        }
    }
}