using LineControl.Domain.Enums;

namespace LineControl.Domain.Entities;

public record TagValue(
    object Value,
    TagType Type
);
