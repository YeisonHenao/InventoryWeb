using InventarioProyecto.Models;
using InventarioProyecto.Services;
using Microsoft.AspNetCore.Mvc;
using InventarioProyecto.Constants;

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
                ErrorTypesAuth.EmailAlreadyRegistered => new Response { status = 0, Message = AuthMessages.EmailAlreadyExists, Data = null },
                _ => new Response { status = 0, Message = GenericMessages.SomethingWentWrong, Data = null }
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
                ErrorTypesAuth.EmailAlreadyRegistered => new Response { status = 0, Message = AuthMessages.EmailAlreadyExists, Data = null },
                ErrorTypesAuth.InvalidCredentials => new Response { status = 0, Message = AuthMessages.InvalidPassword, Data = null },
                ErrorTypesAuth.UserNotFound => new Response { status = 0, Message = AuthMessages.UserNotFound, Data = null },
                ErrorTypesAuth.PasswordMismatch => new Response { status = 0, Message = AuthMessages.InvalidPassword, Data = null },
                ErrorTypesAuth.TokenGenerationFailed => new Response { status = 0, Message = AuthMessages.ErrorGeneratingToken, Data = null },
                ErrorTypesAuth.UserAccessUpdateFailed => new Response { status = 0, Message = AuthMessages.ErrorUpdatingUserAccess, Data = null },
                _ => new Response { status = 0, Message = GenericMessages.SomethingWentWrong, Data = null }
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
    public async Task<IActionResult> RememberPassword([FromBody]RememberPasswordDto request)
    {
        try
        {
            var result = await authService.SolicitarRecuperacion(request.Email);
            return Ok(result);
        }
        catch (AuthException ex)
        {
            var messageError = ex.Tipo switch
            {
                ErrorTypesAuth.EmailNotRegistered => new Response { status = 0, Message = AuthMessages.EmailNotRegistered, Data= null},
                _ => new Response { status= 0, Message= GenericMessages.SomethingWentWrong, Data = null}
            };


            return ex.Tipo switch
            {
                ErrorTypesAuth.EmailNotRegistered => Unauthorized(messageError),
                _ => StatusCode(500, messageError)
            };
        }
    }
}

// DTOs (Data Transfer Objects) para recibir los datos del JSON
public record UserDto(string Nombre, string Email, string Password);
public record LoginDto(string Email, string Password);

public class RememberPasswordDto
{
    public string Email { get; set; }
}