using Application.Entities;
using Application.ExceptionInfos.Queries.GeExceptionInfoById;
using Domain.Common.Interfaces;

namespace Application.ExceptionInfos.Queries.GetAllExceptionInfo;

public sealed class GetExceptionInfoByIdQuerieHandler
{
    private readonly IExceptionInfoRepository _exceptionInfoRepository;

    public GetExceptionInfoByIdQuerieHandler(IExceptionInfoRepository exceptionInfoRepository)
    {
        _exceptionInfoRepository = exceptionInfoRepository;
    }

    public ExceptionInfoEntitie Handle(GetExceptionInfoByIdQuerie querie)
    {
        return _exceptionInfoRepository.GetExceptionInfoByID(exceptionInfoId: querie.Id);
    }
}
