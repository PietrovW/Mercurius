using Domain.Common.Interfaces;
using Domain.Events;

namespace Application.ExceptionInfos.Commands.CreateExceptionInfoItem;

public sealed class CreateExceptionInfoItemHandler
{
    private readonly IExceptionInfoRepository _exceptionInfoRepository;

    public CreateExceptionInfoItemHandler(IExceptionInfoRepository exceptionInfoRepository)
    {
        _exceptionInfoRepository = exceptionInfoRepository;
    }

    public async Task<ExceptionInfoCreatedEvent> Handle(CreateExceptionInfoItemCommand command, CancellationToken cancellationToken = default(CancellationToken))
    {
        var record = new Entities.ExceptionInfoEntitie()
        {
            InnerException = command.InnerException,
            Message = command.Message,
            Source = command.Source,
            StackTrace = command.StackTrace,
            TargetSite = command.TargetSite,

        };
        
        await _exceptionInfoRepository.InsertExceptionInfoAsync(exceptionInfo: record, cancellationToken: cancellationToken);
        await _exceptionInfoRepository.SaveAsync(cancellationToken: cancellationToken);
        return new ExceptionInfoCreatedEvent() {
            InnerException = command.InnerException,
            Message = command.Message,
            Source = command.Source,
            StackTrace = command.StackTrace,
            TargetSite = command.TargetSite,
            Id= record.Id
        };
    }
}
