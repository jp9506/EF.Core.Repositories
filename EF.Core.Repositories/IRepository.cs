using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    /// <summary>
    /// Provides read/write functionality to a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of data in the repository.</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T>
        where T : class
    {
    }

    internal interface IInternalRepository<T> : IRepository<T>, IInternalReadOnlyRepository<T>
        where T : class
    {
        Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default);
    }
}