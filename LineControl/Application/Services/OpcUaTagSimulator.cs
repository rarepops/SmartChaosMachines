using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using System.Collections.Concurrent;

namespace LineControl.Application.Services;

public class OpcUaTagSimulator : IDisposable
{
    private readonly ConcurrentDictionary<string, TagValue> _tags = new();
    private readonly ILogger<OpcUaTagSimulator> _logger;
    private readonly Timer _simulationTimer;
    private volatile bool _isConnected;
    private volatile bool _disposed;

    public bool IsConnected => _isConnected;

    public OpcUaTagSimulator(ILogger<OpcUaTagSimulator> logger)
    {
        _logger = logger;
        InitializeDefaultTags();

        _simulationTimer = new Timer(SimulateTagUpdates, null,
            TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
    }

    public Task<bool> ConnectAsync(string endpoint)
    {
        _logger.LogInformation("Simulating OPC-UA connection to {Endpoint}", endpoint);
        _isConnected = true;
        return Task.FromResult(true);
    }

    public Task DisconnectAsync()
    {
        _isConnected = false;
        _logger.LogInformation("Simulating OPC-UA disconnection");
        return Task.CompletedTask;
    }

    public Task<T> ReadTagAsync<T>(string tagName)
    {
        if (!_isConnected)
            throw new InvalidOperationException("Not connected to OPCUA server");

        if (!_tags.TryGetValue(tagName, out var tagValue))
            throw new ArgumentException($"Tag '{tagName}' not found");

        if (tagValue.Value is not T)
            throw new InvalidCastException($"Tag '{tagName}' is of type {tagValue.Type}, not {typeof(T).Name}");

        return Task.FromResult((T)tagValue.Value);
    }

    public Task WriteTagAsync<T>(string tagName, T value)
    {
        if (!_isConnected)
            throw new InvalidOperationException("Not connected to OPCUA server");

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var tagType = GetTagTypeFromValue(value);

        _tags.AddOrUpdate(tagName,
            new TagValue(value, tagType),
            (key, existingValue) => new TagValue(value, tagType));

        _logger.LogDebug("Written tag {TagName} = {Value} ({Type})", tagName, value, tagType);
        return Task.CompletedTask;
    }

    public Dictionary<string, TagValue> GetAllTags()
    {
        return new Dictionary<string, TagValue>(_tags);
    }

    private void InitializeDefaultTags()
    {
        _tags["RecipeId"] = new TagValue("", TagType.String);
        _tags["CurrentCount"] = new TagValue(0, TagType.Int);
        _tags["Status"] = new TagValue("Idle", TagType.String);
        _tags["ElementsPerBag"] = new TagValue(24, TagType.Int);
        _tags["CountingSpeed"] = new TagValue(0, TagType.Int);
        _tags["BeltSpeed"] = new TagValue(50, TagType.Int);
        _tags["VibrationStrength"] = new TagValue(2, TagType.Int);
        _tags["IsUsedInOrder"] = new TagValue(false, TagType.Bool);
        _tags["HasError"] = new TagValue(false, TagType.Bool);
    }

    private void SimulateTagUpdates(object? state)
    {
        if (_disposed || !_isConnected) return;

        try
        {
            var status = (string)_tags["Status"].Value;
            var currentCount = (int)_tags["CurrentCount"].Value;
            var isUsedInOrder = (bool)_tags["IsUsedInOrder"].Value;

            // Only simulate updates when machine is running
            if (status == "Running" && isUsedInOrder)
            {
                // Increment count realistically
                var increment = Random.Shared.Next(1, 4);
                var newCount = currentCount + increment;
                _tags["CurrentCount"] = new TagValue(newCount, TagType.Int);

                // Simulate speed variation
                var currentSpeed = (int)_tags["CountingSpeed"].Value;
                var speedVariation = Random.Shared.Next(-2, 3);
                var newSpeed = Math.Max(0, Math.Min(100, currentSpeed + speedVariation));
                _tags["CountingSpeed"] = new TagValue(newSpeed, TagType.Int);

                // Simulate completing batches
                var elementsPerBag = (int)_tags["ElementsPerBag"].Value;
                if (newCount >= elementsPerBag * 10) // 10 bags completed
                {
                    _tags["Status"] = new TagValue("Completed", TagType.String);
                    _tags["CountingSpeed"] = new TagValue(0, TagType.Int);
                }
            }

            // Occasionally simulate errors (1% chance)
            if (Random.Shared.NextDouble() < 0.01)
            {
                _tags["HasError"] = new TagValue(true, TagType.Bool);
                _tags["Status"] = new TagValue("Error", TagType.String);
            }
            else if ((string)_tags["Status"].Value == "Error" && Random.Shared.NextDouble() < 0.1)
            {
                // 10% chance to recover from error
                _tags["HasError"] = new TagValue(false, TagType.Bool);
                _tags["Status"] = new TagValue("Running", TagType.String);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during tag simulation");
        }
    }

    private static TagType GetTagTypeFromValue(object value)
    {
        return value.GetType().Name switch
        {
            nameof(Int32) => TagType.Int,
            nameof(Double) => TagType.Double,
            nameof(String) => TagType.String,
            nameof(Boolean) => TagType.Bool,
            nameof(DateTime) => TagType.DateTime,
            _ => throw new ArgumentException($"Unsupported tag type: {value.GetType().Name}")
        };
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _simulationTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
