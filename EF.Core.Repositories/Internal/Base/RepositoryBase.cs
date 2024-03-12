using Microsoft.EntityFrameworkCore;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class RepositoryBase<T> : ReadOnlyRepositoryBase<T>, IInternalRepository<T>
        where T : class
    {
        protected RepositoryBase(IRepositoryFactory factory) : base(factory)
        {
        }

        public abstract Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default);
    }
}