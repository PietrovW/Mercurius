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
       var item = _context.ExceptionInfos.Where(s=>s.Id==exceptionInfoId).FirstOrDefault();

        if (item != null)
        {
            return item;
        }
        return default(ExceptionInfoEntitie);
    }

    public IEnumerable<ExceptionInfoEntitie> GetExceptionInfos()
    {
       return _context.ExceptionInfos.ToList();
    }

    public void InsertExceptionInfo(ExceptionInfoEntitie exceptionInfo)
    {
        _context.ExceptionInfos.Add(exceptionInfo);
    }

    public void Save()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {

        }
    }

    public void UpdateExceptionInfo(ExceptionInfoEntitie exceptionInfo)
    {
        throw new NotImplementedException();
    }
}
