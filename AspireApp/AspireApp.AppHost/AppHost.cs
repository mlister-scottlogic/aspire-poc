using AspireApp.Constants;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres(AspireConstants.Postgres, port: 5432);
var postgresdb = postgres.AddDatabase(AspireConstants.Database);

var rabbitmq = builder.AddRabbitMQ(AspireConstants.Messaging).WithManagementPlugin(port: 53354);

var apiService = builder
    .AddProject<Projects.AspireApp_ApiService>(AspireConstants.Api)
    .WithHttpHealthCheck("/health")
    .WithReference(postgresdb)
    .WithReference(rabbitmq)
    .WaitFor(postgresdb)
    .WaitFor(rabbitmq);

builder
    .AddProject<Projects.AspireApp_Web>(AspireConstants.Frontend)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
