using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class ReadOnlyRepositoryBase<T> : IInternalReadOnlyRepository<T>
    {
        protected ReadOnlyRepositoryBase(IRepositoryFactory factory)
        {
            Factory = factory;
        }

        public IRepositoryFactory Factory { get; }

        public abstract IQueryable<T> EntityQuery(DbContext context);
    }
}