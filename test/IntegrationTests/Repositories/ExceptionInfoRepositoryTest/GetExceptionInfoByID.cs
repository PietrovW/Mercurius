using Domain.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Repositories.ExceptionInfoRepositoryTest;

public class GetExceptionInfoByID
{
    private readonly MercuriusContext _mercuriusContext;
    private readonly IExceptionInfoRepository _exceptionInfoRepository;

    public GetExceptionInfoByID(ITestOutputHelper output)
    {
        _output = output;
        var dbOptions = new DbContextOptionsBuilder<MercuriusContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;
        _mercuriusContext = new MercuriusContext(dbOptions);
        _exceptionInfoRepository = new EfRepository<Order>(_mercuriusContext);
    }
}
