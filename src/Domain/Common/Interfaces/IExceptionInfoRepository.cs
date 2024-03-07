using Application.Entities;

namespace Domain.Common.Interfaces;

public interface IExceptionInfoRepository:IDisposable
{
    Task<IEnumerable<ExceptionInfoEntitie>> GetExceptionInfosAsync(CancellationToken cancellationToken = default(CancellationToken));
    ExceptionInfoEntitie GetExceptionInfoByID(Guid exceptionInfoId);
    Task InsertExceptionInfoAsync(ExceptionInfoEntitie exceptionInfo, CancellationToken cancellationToken = default(CancellationToken));
    Task<bool> DeleteExceptionInfoAsync(Guid exceptionInfoId, CancellationToken cancellationToken = default(CancellationToken));
    void UpdateExceptionInfo(ExceptionInfoEntitie exceptionInfo);
    Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
}
