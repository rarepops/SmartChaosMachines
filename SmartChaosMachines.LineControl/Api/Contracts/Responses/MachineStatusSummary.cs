namespace LineControl.Api.Contracts.Responses;

public class MachineStatusSummary
{
    public required string Position { get; set; }
    public required string Status { get; set; }
    public bool HasError { get; set; }
    public int CurrentCount { get; set; }
    public DateTime LastUpdate { get; set; }
}
