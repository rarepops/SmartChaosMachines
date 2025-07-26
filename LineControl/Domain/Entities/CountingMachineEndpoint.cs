namespace LineControl.Domain.Entities;

public record CountingMachineEndpoint
{
    public required string Position { get; init; }
    public required string Endpoint { get; init; }
}