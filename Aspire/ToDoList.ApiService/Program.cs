using Microsoft.EntityFrameworkCore;
using toDoList.ApiService.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// ✅ Cargar variables de entorno desde .env
Env.Load();

// Intentar obtener la cadena desde variables de entorno (Aspire / Docker)
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// Si no existe, intentar usar la definida en appsettings.Development.json
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("postgresdb");
}

// Si aún no existe, usar una cadena por defecto local
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Host=localhost;Port=5432;Database=postgresdb;Username=postgres;Password=1234";
}

Console.WriteLine($"🔗 Cadena de conexión utilizada: {connectionString}");

// --- Integración con Aspire ---
builder.AddServiceDefaults();

// --- Servicios base ---
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// --- Conexión a PostgreSQL ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- CORS (para el frontend local) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- APLICAR MIGRACIONES AUTOMÁTICAMENTE ---
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

app.UseCors("AllowFrontend");
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.MapDefaultEndpoints();

// --- Endpoint de prueba ---
app.MapGet("/weatherforecast", () =>
{
    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

// --- Record para endpoint de ejemplo ---
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
