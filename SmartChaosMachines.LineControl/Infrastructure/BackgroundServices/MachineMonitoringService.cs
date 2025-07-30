using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.BackgroundServices;

public class MachineMonitoringService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<MachineMonitoringService> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<MachineMonitoringService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

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
