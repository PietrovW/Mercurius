namespace Domain.Events;

public record ExceptionInfoCreatedEvent
{
    public required Guid Id { get; init; }
    public required string Message { get; init; }
    public required string Source { get; init; }
    public required string StackTrace { get; init; }
    public required string TargetSite { get; init; }
    public required string InnerException { get; init; }
}
