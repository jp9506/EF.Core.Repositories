using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    /// <summary>
    /// Provides readonly functionality to a <see cref="DbContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of data in the repository.</typeparam>
    public interface IReadOnlyRepository<T>
    {
    }

    internal interface IInternalReadOnlyRepository<T> : IReadOnlyRepository<T>
    {
        IRepositoryFactory Factory { get; }

        IQueryable<T> EntityQuery(DbContext context);
    }
}