using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    public interface IRepositoryFactory
    {
        Task<DbContext> CreateDbContextAsync(CancellationToken cancellationToken = default);
    }

    public interface IRepositoryFactory<TContext> : IRepositoryFactory
        where TContext : DbContext
    {
        new Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default);

        IReadOnlyRepository<T> GetReadOnlyRepository<T>() where T : class;

        IRepository<T> GetRepository<T>() where T : class;
    }
}