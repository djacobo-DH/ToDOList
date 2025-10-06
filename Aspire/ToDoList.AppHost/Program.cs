using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// 🔑 Definimos el password como parámetro secreto
var postgresPassword = builder.AddParameter("postgres-password", value: "1234", secret: true);

// 🐘 Contenedor PostgreSQL
var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithHostPort(5432)
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);
// 📦 Base de datos asociada con provider PostgreSQL
var postgresdb = postgres.AddDatabase("postgresdb");

// 🧩 API Service — Aspire inyecta la connection string automáticamente
var apiService = builder.AddProject<Projects.ToDoList_ApiService>("apiservice")
    .WithReference(postgresdb)
    .WithHttpHealthCheck("/health");

// 💻 Frontend
builder.AddViteApp(name: "todo-frontend", workingDirectory: "../todo-frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithNpmPackageInstallation();

builder.Build().Run();