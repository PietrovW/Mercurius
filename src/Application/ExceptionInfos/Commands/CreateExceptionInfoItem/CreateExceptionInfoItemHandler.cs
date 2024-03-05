using Domain.Events;

namespace Application.ExceptionInfos.Commands.CreateExceptionInfoItem;

public class CreateExceptionInfoItemHandler
{
    public ExceptionInfoCreatedEvent Handle(CreateExceptionInfoItemCommand command)
    {
        return new ExceptionInfoCreatedEvent();
    }
}
