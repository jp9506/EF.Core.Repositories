using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
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