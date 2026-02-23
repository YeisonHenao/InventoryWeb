using System.Text;
using InventarioProyecto.Data;
using InventarioProyecto.Repositories;
using InventarioProyecto.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE JWT ---
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

// 1. SERVICIOS DE SISTEMA
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. BASE DE DATOS (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. CONFIGURACIÓN DE AUTENTICACIÓN (JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

// 4. INYECCIÓN DE DEPENDENCIAS
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. CONFIGURACIÓN DE CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VitePolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 6. CONFIGURACIÓN DEL PIPELINE (Middleware)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("VitePolicy");
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

// --- EL ORDEN AQUÍ ES CRÍTICO ---
app.UseAuthentication(); // Primero: ¿Quién eres? (Lee el Token)
app.UseAuthorization();  // Segundo: ¿Qué puedes hacer?

// 7. MAPEO DE RUTAS
app.MapControllers();

// 8. FALLBACK PARA VUE
app.MapFallbackToFile("index.html");

app.Run();