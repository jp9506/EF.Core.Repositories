using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class RepositoryBase<T> : ReadOnlyRepositoryBase<T>, IInternalRepository<T>
        where T : class
    {
        protected RepositoryBase(IInternalTransaction transaction) : base(transaction)
        {
        }

        public abstract Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default);
    }
}