using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories
{
    public interface IReadOnlyRepository<T>
    {
    }

    internal interface IInternalReadOnlyRepository<T> : IReadOnlyRepository<T>
    {
        IRepositoryFactory Factory { get; }

        IQueryable<T> EntityQuery(DbContext context);
    }
}