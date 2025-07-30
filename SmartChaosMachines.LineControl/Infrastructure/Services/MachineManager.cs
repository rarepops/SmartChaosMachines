using LineControl.Domain.Interfaces;
using System.Collections.Concurrent;

namespace LineControl.Infrastructure.Services;

public class MachineManager : IMachineManager, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MachineManager> _logger;
    private readonly ConcurrentDictionary<string, ICountingMachine> _machines = new();

    public MachineManager(IServiceScopeFactory serviceScopeFactory, ILogger<MachineManager> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<ICountingMachine> GetOrCreateMachineAsync(string position)
    {
        if (_machines.TryGetValue(position, out var existingMachine))
        {
            return existingMachine;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<ICountingMachineFactory>();

        var newMachine = factory.CreateMachine(position);
        if (_machines.TryAdd(position, newMachine))
        {
            _logger.LogInformation("Created new machine instance for position {Position}", position);
            return newMachine;
        }
        else
        {
            // Another thread might have added it concurrently
            newMachine?.Dispose(); // Clean up the unused instance
            return _machines[position];
        }
    }

    public async Task DisconnectAllAsync()
    {
        foreach (var (position, machine) in _machines)
        {
            try
            {
                if (machine is SimulatedCountingMachine simMachine)
                {
                    await simMachine.DisconnectAsync();
                }
                machine.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting machine at position {Position}", position);
            }
        }
        _machines.Clear();
    }

    public void Dispose()
    {
        DisconnectAllAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }
}
