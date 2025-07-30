using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.Services;

public class SimulatedCountingMachine : ICountingMachine, IDisposable
{
    private readonly ILogger<SimulatedCountingMachine> _logger;
    private readonly OpcUaTagSimulator _tagSimulator;
    private readonly Timer _dataUpdateTimer;
    private volatile bool _disposed;

    public CountingMachineEndpoint Endpoint { get; }
    public CountingMachineState State { get; private set; } = CountingMachineState.Disconnected;

    public event EventHandler<CountingMachineDataChangedEventArgs>? DataChanged;

    public SimulatedCountingMachine(
        CountingMachineEndpoint endpoint,
        ILogger<SimulatedCountingMachine> logger,
        ILoggerFactory loggerFactory)
    {
        Endpoint = endpoint;
        _logger = logger;
        _tagSimulator = new OpcUaTagSimulator(loggerFactory.CreateLogger<OpcUaTagSimulator>());

        // Timer to simulate continuous data updates (every 3 seconds)
        _dataUpdateTimer = new Timer(SimulateDataChanges, null,
            Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    // Implement ICountingMachine interface methods
    public async Task<Dictionary<string, TagValue>> ReadTagsAsync()
    {
        if (State != CountingMachineState.Connected)
        {
            // Auto-connect if not connected
            await ConnectAsync();
        }

        return _tagSimulator.GetAllTags();
    }

    public async Task WriteTagsAsync(Dictionary<string, TagValue> tags)
    {
        if (State != CountingMachineState.Connected)
        {
            await ConnectAsync();
        }

        foreach (var tag in tags)
        {
            await _tagSimulator.WriteTagAsync(tag.Key, tag.Value.Value);
        }
    }

    // Keep existing methods for backward compatibility
    public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (State != CountingMachineState.Disconnected)
        {
            _logger.LogWarning("Machine {Position} is already connected or connecting", Endpoint.Position);
            return State == CountingMachineState.Connected;
        }

        State = CountingMachineState.Connecting;
        _logger.LogInformation("Connecting to counting machine {Position} at {Endpoint}",
            Endpoint.Position, Endpoint.Endpoint);

        try
        {
            // Simulate connection delay
            await Task.Delay(1500, cancellationToken);

            // Connect to simulated OPC-UA server
            await _tagSimulator.ConnectAsync(Endpoint.Endpoint);

            State = CountingMachineState.Connected;

            // Start the data simulation timer
            _dataUpdateTimer.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3));

            _logger.LogInformation("Successfully connected to counting machine {Position}", Endpoint.Position);

            // Fire initial data event
            var initialData = await ReadDataAsync();
            DataChanged?.Invoke(this, new CountingMachineDataChangedEventArgs(initialData));

            return true;
        }
        catch (Exception ex)
        {
            State = CountingMachineState.Error;
            _logger.LogError(ex, "Failed to connect to counting machine {Position}", Endpoint.Position);
            return false;
        }
    }

    public async Task DisconnectAsync()
    {
        _logger.LogInformation("Disconnecting from counting machine {Position}", Endpoint.Position);

        // Stop the simulation timer
        _dataUpdateTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

        // Disconnect from simulated OPC-UA server
        await _tagSimulator.DisconnectAsync();

        State = CountingMachineState.Disconnected;

        _logger.LogInformation("Disconnected from counting machine {Position}", Endpoint.Position);
    }

    public async Task<CountingMachineData> ReadDataAsync()
    {
        if (State != CountingMachineState.Connected)
            throw new InvalidOperationException($"Cannot read data - machine {Endpoint.Position} is not connected");

        try
        {
            // Get all current tag values from the simulator
            var allTags = _tagSimulator.GetAllTags();

            var data = new CountingMachineData(
                Endpoint.Position,
                DateTime.UtcNow,
                allTags
            );

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading data from counting machine {Position}", Endpoint.Position);
            throw;
        }
    }

    private async void SimulateDataChanges(object? state)
    {
        if (_disposed || State != CountingMachineState.Connected) return;

        try
        {
            // Read current data and fire change event
            var currentData = await ReadDataAsync();
            DataChanged?.Invoke(this, new CountingMachineDataChangedEventArgs(currentData));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data simulation for {Position}", Endpoint.Position);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _dataUpdateTimer?.Dispose();
        _tagSimulator?.Dispose();

        GC.SuppressFinalize(this);
    }
}
