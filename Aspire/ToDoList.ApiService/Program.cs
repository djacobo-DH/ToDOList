using Microsoft.EntityFrameworkCore;
using toDoList.ApiService.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Cargar variables de entorno (.env)
Env.Load();

// ✅ 2. Obtener la cadena de conexión
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("postgresdb")
    ?? "Host=localhost;Port=5432;Database=postgresdb;Username=postgres;Password=1234";

Console.WriteLine($"🔗 Cadena de conexión usada: {connectionString}");

// ✅ 3. Integración con Aspire (si aplica)
builder.AddServiceDefaults();

// ✅ 4. Registrar servicios base
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// ✅ 5. Configurar DbContext (PostgreSQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ✅ 6. Configurar CORS (Frontend React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            // Permite tus orígenes locales (React o Vite)
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// ✅ 7. Aplicar migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("✅ Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error aplicando migraciones: {ex.Message}");
    }
}

// ✅ 8. Middleware
app.UseCors("AllowFrontend");
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();

// ✅ 9. Configuración de entorno
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ✅ 10. Mapear controladores
app.MapControllers();
app.MapDefaultEndpoints();

// ✅ 11. Endpoint de prueba opcional
app.MapGet("/ping", () => Results.Ok(new { message = "🟢 API de ToDoList funcionando correctamente." }));

app.Run();
