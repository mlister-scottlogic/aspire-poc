using AspireApp.ApiService.Data.Core.ServiceStartup;
using AspireApp.ApiService.Domain.Core.ServiceStartup;
using AspireApp.ApiService.HangfireJobs;
using AspireApp.ApiService.Messaging.Core.ServiceStartup;
using AspireApp.Constants;
using AspireApp.ServiceDefaults;
using Hangfire;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.AddNpgsqlDataSource(AspireConstants.Database);

builder.Services.RegisterData(builder.Configuration).RegisterDomain().RegisterMessaging();

builder.Services.AddHangfire(c => c.UseInMemoryStorage());

// Default is 15 seconds, if we want schedules less than that we need to make this smaller than
// The delay between jobs
builder.Services.AddHangfireServer(o =>
    o.SchedulePollingInterval = TimeSpan.FromMilliseconds(5000)
);

builder.AddRabbitMQClient(connectionName: AspireConstants.Messaging);

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

app.Services.StartupData(isDevelopment);

// Register hangfire jobs
DailyEntryJob.RegisterWithHangfire();

app.MapDefaultEndpoints();

app.Run();
