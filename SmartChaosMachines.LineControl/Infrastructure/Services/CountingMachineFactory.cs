using LineControl.Domain.Entities;
using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.Services;

public class CountingMachineFactory : ICountingMachineFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMachinePositionConfiguration _positionConfig;

    public CountingMachineFactory(IServiceProvider serviceProvider, IMachinePositionConfiguration positionConfig)
    {
        _serviceProvider = serviceProvider;
        _positionConfig = positionConfig;
    }

    public ICountingMachine CreateMachine(string position)
    {
        var endpoint = new CountingMachineEndpoint
        {
            Position = position,
            Endpoint = _positionConfig.GetEndpointForPosition(position)
        };
        return CreateCountingMachine(endpoint);
    }

    public ICountingMachine CreateCountingMachine(CountingMachineEndpoint endpoint)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<SimulatedCountingMachine>>();
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();

        return new SimulatedCountingMachine(endpoint, logger, loggerFactory);
    }
}
