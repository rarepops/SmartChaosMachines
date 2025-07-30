namespace LineControl.Domain.Entities;

public record CountingMachineData(
    string Position,
    DateTime Timestamp,
    Dictionary<string, TagValue> Tags
);
