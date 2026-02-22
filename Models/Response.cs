using System.ComponentModel.DataAnnotations;

namespace InventarioProyecto.Models;

public class Response
{
    public int status { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}