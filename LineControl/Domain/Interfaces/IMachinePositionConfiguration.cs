namespace LineControl.Domain.Interfaces;

public interface IMachinePositionConfiguration
{
    IReadOnlyList<string> GetAllPositions();
    string GetEndpointForPosition(string position);
}
