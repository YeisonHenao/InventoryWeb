using InventarioProyecto.Models;
using InventarioProyecto.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<Response> Register(UserDto request)
    {
        try
        {
            var result = await authService.Registrar(request);
            if (result != null)
            {
                return result;
            }
            else
            {
                return new Response { status = 0, Message = "Error al registrar el usuario", Data = null };
            }
        }
        catch(Exception ex)
        {
            return new Response { status = 0, Message = ex.Message, Data = null };
        }
    }

    [HttpPost("login")]
    public async Task<Response> Login(LoginDto request)
    {
        try
        {
            var result = await authService.Login(request.Email, request.Password);
            return result;
        }
        catch (Exception ex)
        {
            return new Response { status = 0, Message = ex.Message, Data = null };
        }
    }
}

// DTOs (Data Transfer Objects) para recibir los datos del JSON
public record UserDto(string Nombre, string Email, string Password);
public record LoginDto(string Email, string Password);