namespace LineControl.Domain.Interfaces;

public interface IMachineHealthMonitor
{
    Task<bool> IsHealthyAsync(string position);
    Task<Dictionary<string, bool>> GetAllHealthStatusAsync();
}
