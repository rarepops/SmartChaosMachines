using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.Services;

public class MachineHealthMonitor : IMachineHealthMonitor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MachineHealthMonitor> _logger;

    public MachineHealthMonitor(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MachineHealthMonitor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<bool> IsHealthyAsync(string position)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var machineManager = scope.ServiceProvider.GetRequiredService<IMachineManager>();

            var machine = await machineManager.GetOrCreateMachineAsync(position);
            var data = await machine.ReadTagsAsync();

            if (data.TryGetValue("HasError", out var errorTag) && errorTag.Value is bool hasError)
            {
                return !hasError;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed for machine at position {Position}", position);
            return false;
        }
    }

    public async Task<Dictionary<string, bool>> GetAllHealthStatusAsync()
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var positionConfig = scope.ServiceProvider.GetRequiredService<IMachinePositionConfiguration>();

            var positions = positionConfig.GetAllPositions();
            var healthStatus = new Dictionary<string, bool>();

            foreach (var position in positions)
            {
                healthStatus[position] = await IsHealthyAsync(position);
            }

            return healthStatus;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health status for all machines");
            return new Dictionary<string, bool>();
        }
    }
}
