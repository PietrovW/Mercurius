using Application.Entities;
using Domain.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL;

public sealed class ExceptionInfoRepository : IExceptionInfoRepository
{
    private readonly IDbContextFactory<MercuriusContext> _dbContextFactory;
    private readonly MercuriusContext _context;

    public ExceptionInfoRepository(IDbContextFactory<MercuriusContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _context = _dbContextFactory.CreateDbContext();
    }
    public async Task<bool> DeleteExceptionInfoAsync(Guid exceptionInfoId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _context.ExceptionInfos.FirstOrDefaultAsync(s => s.Id == exceptionInfoId, cancellationToken: cancellationToken);
        if (result == null) return false;
        var resultDelete = _context.ExceptionInfos.Remove(result);
        if (resultDelete != null)
        {
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ExceptionInfoEntitie GetExceptionInfoByID(Guid exceptionInfoId)
    {
        var item = _context.ExceptionInfos.Where(s => s.Id == exceptionInfoId).FirstOrDefault();

        if (item != null)
        {
            return item;
        }
        return default(ExceptionInfoEntitie);
    }

    public async Task<IEnumerable<ExceptionInfoEntitie>> GetExceptionInfosAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.ExceptionInfos.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task InsertExceptionInfoAsync(ExceptionInfoEntitie exceptionInfo, CancellationToken cancellationToken = default(CancellationToken))
    {
      await _context.ExceptionInfos.AddAsync(exceptionInfo, cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }

    public void UpdateExceptionInfo(ExceptionInfoEntitie exceptionInfo)
    {
        _context.ExceptionInfos.Update(exceptionInfo);
    }
}
