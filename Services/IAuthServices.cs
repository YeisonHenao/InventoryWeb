using InventarioProyecto.Models;

public interface IAuthService
{
    Task<Response> Registrar(UserDto user);
    Task<Response> Login(string email, string password);
    Task<Response> SolicitarRecuperacion(string email);
}