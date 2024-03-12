using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    public interface IOrderedRepository<T> : IReadOnlyRepository<T>
    {
    }

    internal interface IInternalOrderedRepository<T> : IOrderedRepository<T>, IInternalReadOnlyRepository<T>
    {
        IOrderedQueryable<T> EntityOrdered(DbContext context);
    }
}