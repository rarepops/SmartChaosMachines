using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.Services;

public class MachinePositionConfiguration : IMachinePositionConfiguration
{
    private readonly Dictionary<string, string> _positionEndpoints = new()
    {
        ["CM01_1"] = "opc.tcp://192.168.1.101:4840",
        ["CM01_2"] = "opc.tcp://192.168.1.102:4840",
        ["CM02_1"] = "opc.tcp://192.168.1.201:4840",
        ["CM02_2"] = "opc.tcp://192.168.1.202:4840"
    };

    public IReadOnlyList<string> GetAllPositions()
    {
        return _positionEndpoints.Keys.ToList().AsReadOnly();
    }

    public string GetEndpointForPosition(string position)
    {
        return _positionEndpoints.TryGetValue(position, out var endpoint)
            ? endpoint
            : $"opc.tcp://192.168.1.100:4840"; // Default fallback
    }
}