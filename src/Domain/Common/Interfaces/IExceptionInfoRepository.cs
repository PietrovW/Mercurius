using Application.Entities;

namespace Domain.Common.Interfaces;

public interface IExceptionInfoRepository:IDisposable
{
    IEnumerable<ExceptionInfoEntitie> GetExceptionInfos();
    ExceptionInfoEntitie GetExceptionInfoByID(Guid exceptionInfoId);
    void InsertExceptionInfo(ExceptionInfoEntitie exceptionInfo);
    void DeleteExceptionInfo(Guid exceptionInfoId);
    void UpdateExceptionInfo(ExceptionInfoEntitie exceptionInfo);
    void Save();
}
