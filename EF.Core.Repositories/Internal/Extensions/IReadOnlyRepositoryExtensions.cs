using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Internal.Extensions
{
    internal static class IReadOnlyRepositoryExtensions
    {
        public static async Task<T?> GetAsync<T>(this IInternalReadOnlyRepository<T> repository, DbContext context, object key, CancellationToken cancellationToken = default) where T : class
            => await new ContextQueryable<T>(repository.EntityQuery(context), context).FindAsync(key, cancellationToken);
    }
}