var builder = DistributedApplication.CreateBuilder(args);

var lineControl = builder.AddProject<Projects.SmartChaosMachines_LineControl>("linecontrol")
    .WithHttpEndpoint(port: 5000, name: "http-api")
    .WithHttpsEndpoint(port: 5001, name: "https-api");

lineControl
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.Build().Run();
