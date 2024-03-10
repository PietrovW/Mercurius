namespace Domain.Exceptions;

public sealed class DuplicateException : Exception
{
    public DuplicateException(string message) : base(message)
    {
    }
}
