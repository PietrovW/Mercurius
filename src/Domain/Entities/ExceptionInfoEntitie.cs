using Application.Interfaces;
using Domain.Common;

namespace Application.Entities;

public record ExceptionInfoEntitie: BaseEntity, IAggregateRoot
{
    public required string Message { get; init; }
    public required string Source { get; init; }
    public required string StackTrace { get; init; }
    public required string TargetSite { get; init; }
    public required string InnerException { get; init; }
}
