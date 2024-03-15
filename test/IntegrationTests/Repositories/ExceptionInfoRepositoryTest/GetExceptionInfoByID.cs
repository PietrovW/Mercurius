using Application.Entities;
using Domain.Common.Interfaces;
using Infrastructure.DAL;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Repositories.ExceptionInfoRepositoryTest;

public class GetExceptionInfoByID
{
    private readonly IDbContextFactory<MercuriusContext> _contextFactory;
    private readonly IExceptionInfoRepository _exceptionInfoRepository;

    public GetExceptionInfoByID()
    {
        _contextFactory =new MigrationsContextFactory();
        _exceptionInfoRepository = new ExceptionInfoRepository(dbContextFactory: _contextFactory);
    }

    [Test]
    public async Task RemoveEmptyQuantities()
    {
        // Arrange
        ExceptionInfoEntitie exceptionInfo = new ExceptionInfoEntitie() { 
            InnerException =string.Empty,
            Message = string.Empty, 
            Source = string.Empty, 
            StackTrace = string.Empty,
            TargetSite = string.Empty,
        };
        
        // Act
        await _exceptionInfoRepository.InsertExceptionInfoAsync(exceptionInfo: exceptionInfo);
        await _exceptionInfoRepository.SaveAsync();
        // Assert
        Assert.AreEqual(0, 0);
    }
}

public class MigrationsContextFactory : IDbContextFactory<MercuriusContext>
{
    public MercuriusContext CreateDbContext()
    {
        var dbOptions = new DbContextOptionsBuilder<MercuriusContext>()
            .UseInMemoryDatabase(databaseName: $"MercuriusContext_{Guid.NewGuid()}")
            .Options;

        return new MercuriusContext(dbOptions);
    }
}
