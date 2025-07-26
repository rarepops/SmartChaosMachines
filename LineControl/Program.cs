using LineControl.Application.Services;
using LineControl.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add CORS configuration (ADD THIS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Smart Chaos Machines - Line Control API",
        Version = "v1",
        Description = "API for managing counting machines in the Smart Chaos Machines platform"
    });
});

// Register your domain services
builder.Services.AddSingleton<ICountingMachineFactory, CountingMachineFactory>();
builder.Services.AddSingleton<LineControlService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<LineControlService>());

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: Add CORS middleware BEFORE routing and controllers
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
