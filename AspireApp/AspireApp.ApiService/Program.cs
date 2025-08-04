using AspireApp.ApiService.Data.Core.ServiceStartup;
using AspireApp.ApiService.Domain.Core.ServiceStartup;
using AspireApp.ApiService.Messaging.Core.ServiceStartup;
using AspireApp.ServiceDefaults;
using Hangfire;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.AddNpgsqlDataSource("postgresdb");

builder.Services.RegisterData(builder.Configuration).RegisterDomain();

builder.Services.AddHangfire(c => c.UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseHangfireDashboard();

var isDevelopment = app.Environment.IsDevelopment();

if (isDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Services.StartupData(isDevelopment).StartupMessaging();

app.MapDefaultEndpoints();

app.Run();
