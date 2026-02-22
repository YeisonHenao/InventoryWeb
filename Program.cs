using Microsoft.EntityFrameworkCore;
using InventarioProyecto.Data; // Ajusta según tu namespace
using InventarioProyecto.Repositories;
using InventarioProyecto.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICIOS DE SISTEMA
builder.Services.AddControllers(); // Habilita el uso de Controladores
builder.Services.AddOpenApi();

// 2. BASE DE DATOS (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. INYECCIÓN DE DEPENDENCIAS (Patrón Repositorio y Servicio)
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 4. CONFIGURACIÓN DE CORS (Para desarrollo con Vite)
builder.Services.AddCors(options =>
{
    options.AddPolicy("VitePolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Puerto por defecto de Vite
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 5. CONFIGURACIÓN DEL PIPELINE (Middleware)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("VitePolicy"); // Activa CORS solo en desarrollo
}

app.UseHttpsRedirection();

// --- EL PUENTE CON EL FRONTEND ---
// 6. Permitir que .NET sirva archivos como index.html, .js, .css
app.UseDefaultFiles(); // Busca index.html por defecto
app.UseStaticFiles();  // Sirve los archivos de wwwroot

app.UseAuthorization();

// 7. MAPEO DE RUTAS
app.MapControllers(); // Mapea los controladores de la carpeta /Controllers

// 8. FALLBACK (La magia del SPA)
// Si el usuario recarga la página o entra a una ruta que no es de API,
// .NET le envía el index.html de Vue para que Vue maneje el routing.
app.MapFallbackToFile("index.html");

app.Run();