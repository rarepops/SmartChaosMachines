using LineControl.Application.Services;
using LineControl.Application.UseCases.ConfigureMachine;
using LineControl.Application.UseCases.GetAllMachines;
using LineControl.Application.UseCases.GetMachineData;
using LineControl.Application.Validators;
using LineControl.Domain.Interfaces;
using LineControl.Infrastructure.BackgroundServices;
using LineControl.Infrastructure.Repositories;
using LineControl.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging
builder.Services.AddLogging();

builder.Services.AddSingleton<IMachinePositionConfiguration, MachinePositionConfiguration>();

// Domain Layer Services
builder.Services.AddScoped<ICountingMachineFactory, CountingMachineFactory>();

// Application Layer Services
builder.Services.AddScoped<MachineDataProcessor>();
builder.Services.AddScoped<ConfigurationProcessor>();
builder.Services.AddScoped<IConfigurationValidator, ConfigurationRequestValidator>();

// Application Layer Use Cases
builder.Services.AddScoped<GetMachineDataUseCase>();
builder.Services.AddScoped<ConfigureMachineUseCase>();
builder.Services.AddScoped<GetAllMachinesUseCase>();

builder.Services.AddSingleton<IMachineManager, MachineManager>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineHealthMonitor, MachineHealthMonitor>();

// Background Services
builder.Services.AddHostedService<MachineMonitoringService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LineControl API V1");
        c.EnableTryItOutByDefault();
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.DisplayRequestDuration();
        c.EnableFilter();

        c.ConfigObject.TryItOutEnabled = false;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
