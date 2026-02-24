using InventarioProyecto.Models;
using InventarioProyecto.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
        try
        {
            var result = await authService.Registrar(request);
            return Ok(result);
        }
        catch(AuthException ex)
        {
            var messageError = ex.Tipo switch
            {
                ErrorTypesAuth.EmailAlreadyRegistered => new Response { status = 0, Message = "El correo electrónico ya existe", Data = null },
                _ => new Response { status = 0, Message = "Algo salio mal con el registro", Data = null }
            };

            return ex.Tipo switch
            {
                ErrorTypesAuth.EmailAlreadyRegistered => Unauthorized(messageError),
                _ => StatusCode(500, messageError)
            };
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        try
        {
            var result = await authService.Login(request.Email, request.Password);
            return Ok(result);
        }
        catch (AuthException ex)
        {
            var messageError = ex.Tipo switch
            {
                ErrorTypesAuth.EmailAlreadyRegistered => new Response { status = 0, Message = "El correo electrónico ya está registrado", Data = null },
                ErrorTypesAuth.InvalidCredentials => new Response { status = 0, Message = "Credenciales inválidas", Data = null },
                ErrorTypesAuth.UserNotFound => new Response { status = 0, Message = "Usuario no encontrado", Data = null },
                ErrorTypesAuth.PasswordMismatch => new Response { status = 0, Message = "Contraseña incorrecta", Data = null },
                ErrorTypesAuth.TokenGenerationFailed => new Response { status = 0, Message = "Error al generar el token", Data = null },
                ErrorTypesAuth.UserAccessUpdateFailed => new Response { status = 0, Message = "Error al actualizar el acceso del usuario", Data = null },
                _ => new Response { status = 0, Message = "Error de autenticación", Data = null }
            };

            return ex.Tipo switch
            {
                ErrorTypesAuth.EmailNotRegistered => NotFound(messageError),
                ErrorTypesAuth.EmailAlreadyRegistered => Conflict(messageError),
                ErrorTypesAuth.InvalidCredentials => BadRequest(messageError),
                ErrorTypesAuth.UserNotFound => NotFound(messageError),
                ErrorTypesAuth.PasswordMismatch => Unauthorized(messageError),
                ErrorTypesAuth.TokenGenerationFailed => StatusCode(500, messageError),
                ErrorTypesAuth.UserAccessUpdateFailed => StatusCode(500, messageError),
                _ => StatusCode(500, messageError)
            };
        }
    }

    [HttpPost("RememberPassword")]
    public async Task<Response> RememberPassword(string email)
    {
        try
        {
            var result = await authService.SolicitarRecuperacion(email);
            return result;
        }
        catch (AuthException ex)
        {
            return ex.Tipo switch
            {
                ErrorTypesAuth.EmailNotRegistered => new Response { status = 0, Message = "El correo electrónico no está registrado", Data = null },
                _ => new Response { status = 0, Message = "Error al solicitar recuperación de contraseña", Data = null }
            };
        }
    }
}

// DTOs (Data Transfer Objects) para recibir los datos del JSON
public record UserDto(string Nombre, string Email, string Password);
public record LoginDto(string Email, string Password);