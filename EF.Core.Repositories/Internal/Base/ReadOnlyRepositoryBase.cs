using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class ReadOnlyRepositoryBase<T>(IInternalTransaction transaction) : IInternalReadOnlyRepository<T>
    {
        public IInternalTransaction Transaction { get; } = transaction;

        public abstract IQueryable<T> EntityQuery(DbContext context);
    }
}