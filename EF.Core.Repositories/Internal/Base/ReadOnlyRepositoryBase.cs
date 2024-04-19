using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class ReadOnlyRepositoryBase<T> : IInternalReadOnlyRepository<T>
    {
        protected ReadOnlyRepositoryBase(IInternalTransaction transaction)
        {
            Transaction = transaction;
        }

        public IInternalTransaction Transaction { get; }

        public abstract IQueryable<T> EntityQuery(DbContext context);
    }
}