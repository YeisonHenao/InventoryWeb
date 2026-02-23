using InventarioProyecto.Models;

namespace InventarioProyecto.Repositories;

public interface IAuthRepository
{
    Task<Usuario> AuthenticateAsync(string username, string password);
    Task<Usuario> Register(UserDto entity);
    Task<UsuarioResponse> Login(string Nombre, String Password);
    Task<string> SolicitarRecuperacion(string email);
}