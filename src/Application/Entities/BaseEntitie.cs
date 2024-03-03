using Application.Interfaces;

namespace Application.Entities;

public record BaseEntitie
{
    public Guid Id { get; init; }
    public DateTimeOffset Creat { get; init; } = DateTimeOffset.Now;
}
