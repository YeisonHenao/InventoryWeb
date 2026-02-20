using InventarioProyecto.Models;

public interface IAuthService
{
    Task<string> Registrar(Usuario user, string password);
    Task<string> Login(string email, string password);
    Task<bool> SolicitarRecuperacion(string email);
}