var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres", port: 5432);
var postgresdb = postgres.AddDatabase("postgresdb");

var rabbitmq = builder.AddRabbitMQ("messaging").WithManagementPlugin(port: 53354);

var apiService = builder
    .AddProject<Projects.AspireApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(postgresdb)
    .WithReference(rabbitmq)
    .WaitFor(postgresdb)
    .WaitFor(rabbitmq);

builder
    .AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
