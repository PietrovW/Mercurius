using Application.Entities;
using Domain.Common.Interfaces;

namespace Application.ExceptionInfos.Queries.GetAllExceptionInfo;

public sealed class GetAllExceptionInfoQuerieHandler
{
    private readonly IExceptionInfoRepository _exceptionInfoRepository;

    public GetAllExceptionInfoQuerieHandler(IExceptionInfoRepository exceptionInfoRepository)
    {
        _exceptionInfoRepository = exceptionInfoRepository;
    }

    public IEnumerable<ExceptionInfoEntitie> Handle(GetAllExceptionInfoQuerie querie)
    {
        return _exceptionInfoRepository.GetExceptionInfos();
    }
}
