using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.BackgroundServices;

public class MachineMonitoringService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MachineMonitoringService> _logger;

    public MachineMonitoringService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MachineMonitoringService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Create a new scope for each monitoring cycle
                using var scope = _serviceScopeFactory.CreateScope();

                // Get scoped services from the scope
                var healthMonitor = scope.ServiceProvider.GetRequiredService<IMachineHealthMonitor>();

                var healthStatus = await healthMonitor.GetAllHealthStatusAsync();
                foreach (var (position, isHealthy) in healthStatus)
                {
                    if (!isHealthy)
                    {
                        _logger.LogWarning("Machine at position {Position} is not healthy", position);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in machine monitoring service");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
