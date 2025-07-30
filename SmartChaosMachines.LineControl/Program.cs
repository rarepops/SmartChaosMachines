using LineControl.Application.Services;
using LineControl.Application.UseCases.ConfigureMachine;
using LineControl.Application.UseCases.GetAllMachines;
using LineControl.Application.UseCases.GetMachineData;
using LineControl.Application.Validators;
using LineControl.Domain.Interfaces;
using LineControl.Infrastructure.BackgroundServices;
using LineControl.Infrastructure.Repositories;
using LineControl.Infrastructure.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Domain Layer Services
builder.Services.AddSingleton<IMachinePositionConfiguration, MachinePositionConfiguration>();
builder.Services.AddScoped<ICountingMachineFactory, CountingMachineFactory>();

// Application Layer Services
builder.Services.AddScoped<MachineDataProcessor>();
builder.Services.AddScoped<ConfigurationProcessor>();
builder.Services.AddScoped<IConfigurationValidator, ConfigurationRequestValidator>();

// Application Layer Use Cases
builder.Services.AddScoped<GetMachineDataUseCase>();
builder.Services.AddScoped<ConfigureMachineUseCase>();
builder.Services.AddScoped<GetAllMachinesUseCase>();

// Infrastructure Layer Services
builder.Services.AddSingleton<IMachineManager, MachineManager>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineHealthMonitor, MachineHealthMonitor>();

// Background Services
builder.Services.AddHostedService<MachineMonitoringService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartChaosMachines LineControl API V1");
        c.DocExpansion(DocExpansion.None);
        c.EnableDeepLinking();
        c.DisplayRequestDuration();
        c.EnableFilter();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
