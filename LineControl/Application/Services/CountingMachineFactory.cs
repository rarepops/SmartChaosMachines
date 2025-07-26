using LineControl.Domain.Entities;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.Services;


public class CountingMachineFactory : ICountingMachineFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CountingMachineFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICountingMachine CreateCountingMachine(CountingMachineEndpoint endpoint)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<SimulatedCountingMachine>>();
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();

        return new SimulatedCountingMachine(endpoint, logger, loggerFactory);
    }
}