using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class RepositoryBase<T>(IInternalTransaction transaction) : ReadOnlyRepositoryBase<T>(transaction), IInternalRepository<T>
        where T : class
    {
        public abstract Task HandleExpressionUpdateAsync(DbContext context, T current, T entity, CancellationToken cancellationToken = default);
    }
}