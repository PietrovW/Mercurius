using Application.Entities;
using Infrastructure.Data;

namespace FunctionalTests;

public class DataSeeder
{
    public static Guid Id = Guid.NewGuid();
    public static void SeedCountries(MercuriusContext context)
    {
        if (!context.ExceptionInfos.Any())
        {
            var exceptionInfos = new List<ExceptionInfoEntitie>();
            exceptionInfos.Add(new ExceptionInfoEntitie
            {
                Id = Id,
                InnerException = $"InnerException_{Guid.NewGuid()}",
                Message = $"Message_{Guid.NewGuid()}",
                Source = $"Source_{Guid.NewGuid()}",
                StackTrace = $"StackTrace_{Guid.NewGuid()}",
                TargetSite = $"TargetSite_{Guid.NewGuid()}",
            });
            for (int i = 0; i < 100; i++)
            {
                exceptionInfos.Add(new ExceptionInfoEntitie
                {
                    Id = Guid.NewGuid(),
                    InnerException = $"InnerException_{Guid.NewGuid()}",
                    Message = $"Message_{Guid.NewGuid()}",
                    Source = $"Source_{Guid.NewGuid()}",
                    StackTrace = $"StackTrace_{Guid.NewGuid()}",
                    TargetSite = $"TargetSite_{Guid.NewGuid()}",
                });
            }

            context.ExceptionInfos.AddRange(exceptionInfos);
            context.SaveChanges();
        }
    }
}
