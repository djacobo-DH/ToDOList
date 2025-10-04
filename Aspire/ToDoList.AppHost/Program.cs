var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.ToDoList_ApiService>("apiservice")
    .WithReference(postgresdb); // ← aquí el punto y coma

builder.AddProject<Projects.ToDoList_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
