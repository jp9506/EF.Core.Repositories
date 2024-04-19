using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EF.Core.Repositories.Internal
{
    internal class RepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _contextFactory;
        private bool disposedValue;

        public RepositoryFactory(IDbContextFactory<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        async Task<DbContext> IRepositoryFactory.GetDbContextAsync(CancellationToken cancellationToken)
        {
            return await GetDbContextAsync(cancellationToken);
        }

        public async Task<TContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            return await _contextFactory.CreateDbContextAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                disposedValue = true;
            }
        }
    }
}