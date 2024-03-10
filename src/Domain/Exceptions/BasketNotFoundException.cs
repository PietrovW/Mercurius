namespace Domain.Exceptions;

public sealed class ExceptionInfoNotFoundException : Exception
{
    public ExceptionInfoNotFoundException(Guid exceptionInfoId) : base($"No ExceptionInfo found with id {exceptionInfoId}")
    {
    }
}
