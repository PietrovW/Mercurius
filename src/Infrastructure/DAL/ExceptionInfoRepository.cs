using Application.Entities;
using Domain.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL;

public class ExceptionInfoRepository : IExceptionInfoRepository
{
    private readonly IDbContextFactory<MercuriusContext> _dbContextFactory;
    private readonly MercuriusContext _context;
    
    public ExceptionInfoRepository(IDbContextFactory<MercuriusContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _context = _dbContextFactory.CreateDbContext();
    }
    public void DeleteExceptionInfo(Guid exceptionInfoId)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ExceptionInfoEntitie GetExceptionInfoByID(Guid exceptionInfoId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ExceptionInfoEntitie> GetExceptionInfos()
    {
       return _context.ExceptionInfos.ToList();
    }

    public void InsertExceptionInfo(ExceptionInfoEntitie exceptionInfo)
    {
        _context.Add(exceptionInfo);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void UpdateExceptionInfo(ExceptionInfoEntitie exceptionInfo)
    {
        throw new NotImplementedException();
    }
}
