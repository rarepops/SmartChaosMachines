using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using System.Collections.Concurrent;
using System.Text;

namespace LineControl.Infrastructure.Services;

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

        _logger.LogInformation("External write: {TagName} = {Value} ({Type})", tagName, value, tagType);
        return Task.CompletedTask;
    }

    public Dictionary<string, TagValue> GetAllTags()
    {
        return new Dictionary<string, TagValue>(_tags);
    }

    private void InitializeDefaultTags()
    {
        string recipeId = GenerateRandomRecipeId();
        int elementsPerBag = Random.Shared.Next(1, 5); // 1 to 4 inclusive

        _tags["RecipeId"] = new TagValue(recipeId, TagType.String);
        _tags["CurrentCount"] = new TagValue(0, TagType.Int);
        _tags["Status"] = new TagValue("Running", TagType.String);
        _tags["ElementsPerBag"] = new TagValue(elementsPerBag, TagType.Int);
        _tags["CountingSpeed"] = new TagValue(10, TagType.Int);
        _tags["BeltSpeed"] = new TagValue(50, TagType.Int);
        _tags["VibrationStrength"] = new TagValue(2, TagType.Int);
        _tags["IsUsedInOrder"] = new TagValue(true, TagType.Bool);
        _tags["HasError"] = new TagValue(false, TagType.Bool);
    }

    private static string GenerateRandomRecipeId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var sb = new StringBuilder(8);
        for (int i = 0; i < 8; i++)
            sb.Append(chars[Random.Shared.Next(chars.Length)]);
        return sb.ToString();
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
                // Increment count by 1 every cycle
                var newCount = currentCount + 1;
                _tags["CurrentCount"] = new TagValue(newCount, TagType.Int);
                _logger.LogInformation("Simulated tag change: CurrentCount = {Value} (Int)", newCount);

                // Simulate speed variation
                var currentSpeed = (int)_tags["CountingSpeed"].Value;
                var speedVariation = Random.Shared.Next(-2, 3);
                var newSpeed = Math.Max(0, Math.Min(100, currentSpeed + speedVariation));
                _tags["CountingSpeed"] = new TagValue(newSpeed, TagType.Int);
                _logger.LogInformation("Simulated tag change: CountingSpeed = {Value} (Int)", newSpeed);

                // Simulate completing batches
                var elementsPerBag = (int)_tags["ElementsPerBag"].Value;
                if (newCount >= elementsPerBag * 10) // 10 bags completed
                {
                    _tags["Status"] = new TagValue("Completed", TagType.String);
                    _tags["CountingSpeed"] = new TagValue(0, TagType.Int);
                    _logger.LogInformation("Simulated tag change: Status = Completed (String)");
                    _logger.LogInformation("Simulated tag change: CountingSpeed = 0 (Int)");
                }
            }

            // Occasionally simulate errors (1% chance)
            if (Random.Shared.NextDouble() < 0.01)
            {
                _tags["HasError"] = new TagValue(true, TagType.Bool);
                _tags["Status"] = new TagValue("Error", TagType.String);
                _logger.LogInformation("Simulated tag change: HasError = true (Bool)");
                _logger.LogInformation("Simulated tag change: Status = Error (String)");
            }
            else if ((string)_tags["Status"].Value == "Error" && Random.Shared.NextDouble() < 0.1)
            {
                // 10% chance to recover from error
                _tags["HasError"] = new TagValue(false, TagType.Bool);
                _tags["Status"] = new TagValue("Running", TagType.String);
                _logger.LogInformation("Simulated tag change: HasError = false (Bool)");
                _logger.LogInformation("Simulated tag change: Status = Running (String)");
            }

            // 10% chance to randomly go idle
            if (Random.Shared.NextDouble() < 0.10)
            {
                _tags["Status"] = new TagValue("Idle", TagType.String);
                _logger.LogInformation("Simulated tag change: Status = Idle (String) -- 10%% random chance");
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
