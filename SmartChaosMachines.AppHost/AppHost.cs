var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.SmartChaosMachines_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.SmartChaosMachines_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.LineControl>("linecontrol");

builder.Build().Run();
