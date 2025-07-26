using LineControl.Api.Controllers;
using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.Services;

public class LineControlService(
    ICountingMachineFactory machineFactory,
    ILogger<LineControlService> logger) : BackgroundService
{
    private readonly ICountingMachineFactory _machineFactory = machineFactory;
    private readonly ILogger<LineControlService> _logger = logger;
    private readonly Dictionary<string, ICountingMachine> _machines = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Line Control Service");

        // Initialize counting machines for the line
        await InitializeCountingMachines();

        // Keep service running
        while (!stoppingToken.IsCancellationRequested)
        {
            // Monitor machine health
            await MonitorMachineHealth();

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task InitializeCountingMachines()
    {
        // Create endpoints for a typical production line (18 machines)
        var endpoints = new[]
        {
            new CountingMachineEndpoint { Position = "CM01_1", Endpoint = "opc.tcp://192.168.1.101:4840" },
            new CountingMachineEndpoint { Position = "CM01_2", Endpoint = "opc.tcp://192.168.1.102:4840" },
            new CountingMachineEndpoint { Position = "CM02_1", Endpoint = "opc.tcp://192.168.1.103:4840" },
            new CountingMachineEndpoint { Position = "CM02_2", Endpoint = "opc.tcp://192.168.1.103:4840" },
        };

        foreach (var endpoint in endpoints)
        {
            try
            {
                var machine = _machineFactory.CreateCountingMachine(endpoint);

                // Subscribe to data changes
                machine.DataChanged += OnMachineDataChanged;

                // Connect to machine
                var connected = await machine.ConnectAsync();
                if (connected)
                {
                    _machines[endpoint.Position] = machine;
                    _logger.LogInformation("Initialized counting machine {Position}", endpoint.Position);
                }
                else
                {
                    _logger.LogWarning("Failed to connect to counting machine {Position}", endpoint.Position);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing counting machine {Position}", endpoint.Position);
            }
        }
    }

    private void OnMachineDataChanged(object? sender, CountingMachineDataChangedEventArgs e)
    {
        var data = e.Data;

        _logger.LogDebug("Machine {Position} data update: Count={Count}, Status={Status}, Speed={Speed}",
            data.Position, data.CurrentCount, data.Status, data.CountingSpeed);

        // Here you would typically:
        // - Send data to message queue (Pulsar) for ML pipeline
        // - Update real-time dashboards
        // - Check for business rule triggers

        if (data.HasError)
        {
            _logger.LogWarning("Machine {Position} reported error in status {Status}",
                data.Position, data.Status);
        }
    }

    private async Task MonitorMachineHealth()
    {
        foreach (var kvp in _machines)
        {
            var position = kvp.Key;
            var machine = kvp.Value;

            if (machine.State == CountingMachineState.Error)
            {
                _logger.LogWarning("Machine {Position} is in error state, attempting reconnection", position);

                try
                {
                    await machine.DisconnectAsync();
                    await Task.Delay(2000);
                    await machine.ConnectAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to reconnect machine {Position}", position);
                }
            }
        }
    }

    public async Task<CountingMachineData?> GetMachineDataAsync(string position)
    {
        if (_machines.TryGetValue(position, out var machine))
        {
            return await machine.ReadDataAsync();
        }
        return null;
    }

    public async Task<bool> ApplyConfigurationAsync(string position, CountingMachineConfig config)
    {
        if (_machines.TryGetValue(position, out var machine))
        {
            try
            {
                await machine.WriteConfigurationAsync(config);
                _logger.LogInformation("Configuration applied to machine {Position}", position);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply configuration to machine {Position}", position);
                return false;
            }
        }
        return false;
    }

    public async Task<List<MachineStatusSummary>> GetAllMachinesStatusAsync()
    {
        var statusList = new List<MachineStatusSummary>();

        foreach (var kvp in _machines)
        {
            var position = kvp.Key;
            var machine = kvp.Value;

            try
            {
                var data = await machine.ReadDataAsync();

                var summary = new MachineStatusSummary(
                    data.Position,
                    data.Status,
                    data.CurrentCount,
                    data.RecipeId,
                    data.HasError
                );

                statusList.Add(summary);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read data from machine {Position}", position);

                // Add a summary with error state if we can't read the machine
                var errorSummary = new MachineStatusSummary(
                    position,
                    "Unreachable",
                    0,
                    "Unknown",
                    true
                );

                statusList.Add(errorSummary);
            }
        }

        return statusList;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Line Control Service");

        // Disconnect all machines
        foreach (var machine in _machines.Values)
        {
            try
            {
                await machine.DisconnectAsync();
                if (machine is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting machine during shutdown");
            }
        }

        await base.StopAsync(cancellationToken);
    }
}
