namespace LineControl.Domain.Entities;

public record CountingMachineConfig(
    string Position,
    Dictionary<string, TagValue> ConfigurationTags
);
