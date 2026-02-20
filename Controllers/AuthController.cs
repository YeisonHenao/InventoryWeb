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
        var user = new Usuario { Nombre = request.Nombre, Email = request.Email };
        return Ok(await authService.Registrar(user, request.Password));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        try {
            var result = await authService.Login(request.Email, request.Password);
            return Ok(new { mensaje = result });
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}

// DTOs (Data Transfer Objects) para recibir los datos del JSON
public record UserDto(string Nombre, string Email, string Password);
public record LoginDto(string Email, string Password);