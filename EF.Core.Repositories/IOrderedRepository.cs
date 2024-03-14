using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    /// <summary>
    /// Represents the result of a sorting operation.
    /// </summary>
    /// <typeparam name="T">The type of data in the repository.</typeparam>
    public interface IOrderedRepository<T> : IReadOnlyRepository<T>
    {
    }

    internal interface IInternalOrderedRepository<T> : IOrderedRepository<T>, IInternalReadOnlyRepository<T>
    {
        IOrderedQueryable<T> EntityOrdered(DbContext context);
    }
}