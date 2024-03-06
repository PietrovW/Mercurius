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

    public ExceptionInfoCreatedEvent Handle(CreateExceptionInfoItemCommand command)
    {
        _exceptionInfoRepository.InsertExceptionInfo(new Entities.ExceptionInfoEntitie() { 
            InnerException=command.InnerException,
            Message=command.Message,
            Source=command.Source,
            StackTrace=command.StackTrace,
            TargetSite=command.TargetSite,
            Id=0,
        });
        return new ExceptionInfoCreatedEvent() { };
    }
}
