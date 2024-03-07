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

    public async Task<IEnumerable<ExceptionInfoEntitie>> Handle(GetAllExceptionInfoQuerie querie, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _exceptionInfoRepository.GetExceptionInfosAsync(cancellationToken: cancellationToken);
    }
}
